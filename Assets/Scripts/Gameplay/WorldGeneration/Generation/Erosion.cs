using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace WorldGen
{
    [System.Serializable]
    public class ErosionData
    {
        [Header("Erosion Settings")]
        public ComputeShader erosion;
        public int numErosionIterations = 50000;
        public int erosionBrushRadius = 3;

        public int maxLifetime = 30;
        public float sedimentCapacityFactor = 3;
        public float minSedimentCapacity = .01f;
        public float depositSpeed = 0.3f;
        public float erodeSpeed = 0.3f;

        public float evaporateSpeed = .01f;
        public float gravity = 4;
        public float startSpeed = 1;
        public float startWater = 1;
        [Range(0, 1)]
        public float inertia = 0.3f;
        public int segmentDivs = 256;
    }


    public static class Erosion
    {
        public static void Generate(WorldEditable worldEditable, ErosionData erosionData, System.Random random)
        {
            int mapSize = erosionData.segmentDivs;
            float[] map = new float[mapSize * mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i + j * mapSize] = worldEditable.heightMap[i, j];
                }
            }

            int mapSizeWithBorder = mapSize + erosionData.erosionBrushRadius * 2;

            // Create brush
            List<int> brushIndexOffsets = new List<int>();
            List<float> brushWeights = new List<float>();

            float weightSum = 0;
            for (int brushY = -erosionData.erosionBrushRadius; brushY <= erosionData.erosionBrushRadius; brushY++)
            {
                for (int brushX = -erosionData.erosionBrushRadius; brushX <= erosionData.erosionBrushRadius; brushX++)
                {
                    float sqrDst = brushX * brushX + brushY * brushY;
                    if (sqrDst < erosionData.erosionBrushRadius * erosionData.erosionBrushRadius)
                    {
                        brushIndexOffsets.Add(brushY * mapSize + brushX);
                        float brushWeight = 1 - Mathf.Sqrt(sqrDst) / erosionData.erosionBrushRadius;
                        weightSum += brushWeight;
                        brushWeights.Add(brushWeight);
                    }
                }
            }
            for (int i = 0; i < brushWeights.Count; i++)
            {
                brushWeights[i] /= weightSum;
            }

            // Send brush data to compute shader
            ComputeBuffer brushIndexBuffer = new ComputeBuffer(brushIndexOffsets.Count, sizeof(int));
            ComputeBuffer brushWeightBuffer = new ComputeBuffer(brushWeights.Count, sizeof(int));
            brushIndexBuffer.SetData(brushIndexOffsets);
            brushWeightBuffer.SetData(brushWeights);
            erosionData.erosion.SetBuffer(0, "brushIndices", brushIndexBuffer);
            erosionData.erosion.SetBuffer(0, "brushWeights", brushWeightBuffer);

            // Generate random indices for droplet placement
            int[] randomIndices = new int[erosionData.numErosionIterations];
            for (int i = 0; i < erosionData.numErosionIterations; i++)
            {
                int randomX = random.Range(erosionData.erosionBrushRadius, mapSize + erosionData.erosionBrushRadius);
                int randomY = random.Range(erosionData.erosionBrushRadius, mapSize + erosionData.erosionBrushRadius);
                randomIndices[i] = randomY * mapSize + randomX;
            }

            // Send random indices to compute shader
            ComputeBuffer randomIndexBuffer = new ComputeBuffer(randomIndices.Length, sizeof(int));
            randomIndexBuffer.SetData(randomIndices);
            erosionData.erosion.SetBuffer(0, "randomIndices", randomIndexBuffer);

            // Heightmap buffer
            ComputeBuffer mapBuffer = new ComputeBuffer(map.Length, sizeof(float));
            mapBuffer.SetData(map);
            erosionData.erosion.SetBuffer(0, "map", mapBuffer);

            // Settings
            erosionData.erosion.SetInt("borderSize", erosionData.erosionBrushRadius);
            erosionData.erosion.SetInt("mapSize", mapSizeWithBorder);
            erosionData.erosion.SetInt("brushLength", brushIndexOffsets.Count);
            erosionData.erosion.SetInt("maxLifetime", erosionData.maxLifetime);
            erosionData.erosion.SetFloat("inertia", erosionData.inertia);
            erosionData.erosion.SetFloat("sedimentCapacityFactor", erosionData.sedimentCapacityFactor);
            erosionData.erosion.SetFloat("minSedimentCapacity", erosionData.minSedimentCapacity);
            erosionData.erosion.SetFloat("depositSpeed", erosionData.depositSpeed);
            erosionData.erosion.SetFloat("erodeSpeed", erosionData.erodeSpeed);
            erosionData.erosion.SetFloat("evaporateSpeed", erosionData.evaporateSpeed);
            erosionData.erosion.SetFloat("gravity", erosionData.gravity);
            erosionData.erosion.SetFloat("startSpeed", erosionData.startSpeed);
            erosionData.erosion.SetFloat("startWater", erosionData.startWater);

            // Run compute shader
            erosionData.erosion.Dispatch(0, 32, 32, 1);
            mapBuffer.GetData(map);

            // Release buffers
            mapBuffer.Release();
            randomIndexBuffer.Release();
            brushIndexBuffer.Release();
            brushWeightBuffer.Release();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    worldEditable.heightMap[i, j] = map[i + j * mapSize];
                }
            }
        }
    }

}