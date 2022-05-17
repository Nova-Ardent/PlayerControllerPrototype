using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using DebugMenu;

namespace WorldGen
{
    public class MarchingCubeChunkGenerator : MonoBehaviour
    {
        struct Triangle
        {
#pragma warning disable 649 // disable unassigned variable warning
            public Vector3 a;
            public Vector3 b;
            public Vector3 c;

            public Vector3 this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0:
                            return a;
                        case 1:
                            return b;
                        default:
                            return c;
                    }
                }
            }
#pragma warning restore 649
        }


        [System.Serializable]
        public class Data
        {
            public int length; // x;
            public int width; // z;
            [Range(1, 10)]
            public int edgeThickness;
            public int chunkAxisSize;
            public float iso;

            [NonSerialized] public int fullLength;
            [NonSerialized] public int fullWidth;
            [NonSerialized] public int chunkAxisReadoutSize;
            [NonSerialized] public int chunkAxsReadoutBlendSize;

            public Material sharedMaterial;
            
            public ComputeShader computeShader;
            public int computeThreadsX = 8;
            public int computeThreadsY = 8;
            public int computeThreadsZ = 8;
            [NonSerialized] public ComputeBuffer triangleBuffer;
            [NonSerialized] public ComputeBuffer triCountBuffer;
            [NonSerialized] public ComputeBuffer pointsBuffer;
            [NonSerialized] public int shaderKernel;
            [NonSerialized] public float[] pointsArray;
            [NonSerialized] public float[] zeroedPointsArray;
            [NonSerialized] public MarchingCubesZerosChunk zeroChunkRef;

            [NonSerialized] bool initialized = false;

            public const int AXIS_READOUT_BLEND_SIZE_OFFSET = 5;
            [NonSerialized] public int[] readoutChunkDArray;
            [NonSerialized] public int[] readoutChunkCArray;

            public void InitNonSerialziedData()
            {
                fullLength = length + edgeThickness * 2;
                fullWidth = width + edgeThickness * 2;
                chunkAxisReadoutSize = chunkAxisSize + 1;
                chunkAxsReadoutBlendSize = chunkAxisReadoutSize + AXIS_READOUT_BLEND_SIZE_OFFSET * 2;

                shaderKernel = computeShader.FindKernel("March");
                pointsArray = new float[chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize];
                zeroedPointsArray = new float[chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize];
                pointsBuffer = new ComputeBuffer(chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize * chunkAxsReadoutBlendSize, sizeof(float));
                triangleBuffer = new ComputeBuffer(chunkAxisReadoutSize * chunkAxisReadoutSize * chunkAxisReadoutSize * 5, sizeof(float) * 3 * 3, ComputeBufferType.Append);
                triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
                zeroChunkRef = new MarchingCubesZerosChunk();

                initialized = true;

                var chunkEdge = (chunkAxisSize - AXIS_READOUT_BLEND_SIZE_OFFSET);
                readoutChunkDArray = new int[chunkAxsReadoutBlendSize];
                readoutChunkCArray = new int[chunkAxsReadoutBlendSize];
                for (int i = 0; i < chunkAxsReadoutBlendSize; i++)
                {
                    readoutChunkDArray[i] = (i + chunkEdge) % chunkAxisSize;
                    readoutChunkCArray[i] = (i + chunkEdge) / chunkAxisSize;
                }
            }

            public void DestroyData()
            {
                if (!initialized)
                {
                    return;
                }

                pointsBuffer.Release();
                triangleBuffer.Release();
                triCountBuffer.Release();
            }
        }

        public float this[int x, int y, int z]
        {
            get => GetPoint(x, y, z);
            set => SetPoint(x, y, z, value);
        }

        [SerializeField] public Data worldData;
        [SerializeField] MarchingCubeChunkLoader marchingCubeChunkLoader;

        MarchingCubesChunkColumn[,] marchingCubeChunks;

        void Start()
        {
            LoadWorld(MarchingCubeChunkLoader.GetWorldFolders().First());
            RegisterDebug();
        }

        private void OnDestroy()
        {
            worldData.DestroyData();
        }

        void Init(string seed, int worldsGenerated)
        {
            worldData.InitNonSerialziedData();
            marchingCubeChunks = new MarchingCubesChunkColumn[worldData.fullLength, worldData.fullWidth];
            for (int i = 0; i < worldData.fullLength; i++)
            {
                for (int j = 0; j < worldData.fullWidth; j++)
                {
                    marchingCubeChunks[i, j] = new MarchingCubesChunkColumn(seed, worldsGenerated, i, j);
                }
            }
        }

        float GetPoint(int x, int y, int z)
        {
            int chunkX = x / worldData.chunkAxisSize + worldData.edgeThickness;
            int chunkY = y / worldData.chunkAxisSize + worldData.edgeThickness;
            int chunkZ = z / worldData.chunkAxisSize;

            if (marchingCubeChunks[chunkX, chunkY].ContainsKey(chunkZ))
            {
                int indexX = x % worldData.chunkAxisSize;
                int indexY = y % worldData.chunkAxisSize;
                int indexZ = z % worldData.chunkAxisSize;
                return marchingCubeChunks[chunkX, chunkY][chunkZ][indexX, indexY, indexZ];
            }
            return 0;
        }

        void SetPoint(int x, int y, int z, float intensity)
        {
            int indexX = x % worldData.chunkAxisSize;
            int chunkX = x / worldData.chunkAxisSize + worldData.edgeThickness;
            
            int indexY = y % worldData.chunkAxisSize;
            int chunkY = y / worldData.chunkAxisSize + worldData.edgeThickness;
            
            int indexZ = z % worldData.chunkAxisSize;
            int chunkZ = z / worldData.chunkAxisSize;

            if (!marchingCubeChunks[chunkX, chunkY].ContainsKey(chunkZ))
            {
                marchingCubeChunks[chunkX, chunkY][chunkZ] = new MarchingCubeChunkData(worldData.chunkAxisSize);
            }

            marchingCubeChunks[chunkX, chunkY][chunkZ][indexX, indexY, indexZ] = intensity;
        }

        public void RegenerateAllChunks()
        {
            List<Transform> tforms = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                tforms.Add(transform.GetChild(i));
            }

            foreach (var child in tforms)
            {
                Destroy(child.gameObject);
            }

            GenerateAllChunks();
        }

        public void GenerateAllChunks()
        {
            Dictionary<Vector3Int, bool> neighbours = new Dictionary<Vector3Int, bool>();
            for (int i = 0; i < worldData.fullLength - 1; i++)
            {
                for (int j = 0; j < worldData.fullWidth - 1; j++)
                {
                    foreach (var key in marchingCubeChunks[i, j].Keys)
                    {
                        GenerateChunk(i - 1, j - 1, key);
                        // need to generate neighbours that are empty, and not already generated.
                        // maybe use a list?
                        AddNeighbours(i - 1, j - 1, key, neighbours);
                    }
                }
            }

            foreach (var neighbour in neighbours.Keys)
            {
                GenerateChunk(neighbour.x, neighbour.y, neighbour.z);
            }
        }

        void AddNeighbours(int x, int y, int z, Dictionary<Vector3Int, bool> neighbours)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (marchingCubeChunks[x - i + 1, y - j + 1].ContainsKey(z - k))
                        {
                            continue;
                        }

                        neighbours[new Vector3Int(x - i, y - j, z - k)] = true;
                    }
                }
            }
        }

#region MustMutex
        void GenerateChunk(int x, int y, int z)
        {
            GameObject chunk = new GameObject($"{x}, {y}, {z}");

            var meshRenderer = chunk.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = worldData.sharedMaterial;

            var meshFilter = chunk.AddComponent<MeshFilter>();
            meshFilter.mesh = GetChunkMesh(x, y, z);

            chunk.transform.SetParent(this.transform);
            chunk.transform.localPosition = new Vector3(x, z, y) * worldData.chunkAxisSize;
            chunk.isStatic = true;
        }

        Mesh GetChunkMesh(int x, int y, int z)
        {
            int numVoxelsPerAxis = worldData.chunkAxisSize;
            int numThreadsPerAxisX = Mathf.CeilToInt(numVoxelsPerAxis / (float)worldData.computeThreadsX);
            int numThreadsPerAxisY = Mathf.CeilToInt(numVoxelsPerAxis / (float)worldData.computeThreadsY);
            int numThreadsPerAxisZ = Mathf.CeilToInt(numVoxelsPerAxis / (float)worldData.computeThreadsZ);

            worldData.pointsBuffer.SetData(updateAndGetChunkPoints(x, y, z));

            worldData.triangleBuffer.SetCounterValue(0);
            worldData.computeShader.SetBuffer(0, "points", worldData.pointsBuffer);
            worldData.computeShader.SetBuffer(0, "triangles", worldData.triangleBuffer);
            worldData.computeShader.SetInt("numPointsPerAxis", worldData.chunkAxisReadoutSize);
            worldData.computeShader.SetInt("numPointsWithThickness", worldData.chunkAxsReadoutBlendSize);
            worldData.computeShader.SetFloat("isoLevel", worldData.iso);

            worldData.computeShader.Dispatch(worldData.shaderKernel, numThreadsPerAxisX, numThreadsPerAxisY, numThreadsPerAxisZ);

            ComputeBuffer.CopyCount(worldData.triangleBuffer, worldData.triCountBuffer, 0);
            int[] triCountArray = { 0 };
            worldData.triCountBuffer.GetData(triCountArray);
            int numTris = triCountArray[0];

            // Get triangle data from shader
            Triangle[] tris = new Triangle[numTris];
            worldData.triangleBuffer.GetData(tris, 0, 0, numTris);

            Mesh mesh = new Mesh();

            var vertices = new Vector3[numTris * 3];
            var meshTriangles = new int[numTris * 3];

            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    meshTriangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i][j];
                }
            }
            mesh.vertices = vertices;
            mesh.triangles = meshTriangles;

            mesh.RecalculateNormals();
            return mesh;
        }

        float[] updateAndGetChunkPoints(int x, int y, int z)
        {
            MarchingCubesChunkBase[,,] neighbouringChunks = new MarchingCubesChunkBase[3,3,3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int dx = x + i - 1 + worldData.edgeThickness;
                        int dy = y + j - 1 + worldData.edgeThickness;
                        int dz = z + k - 1;
                        if (dx < 0 || dy < 0 || dz < 0)
                        {
                            neighbouringChunks[i, j, k] = new MarchingCubesZerosChunk();
                        }
                        else if (marchingCubeChunks[dx, dy].ContainsKey(dz))
                        {
                            neighbouringChunks[i, j, k] = marchingCubeChunks[dx, dy][dz];
                        }
                        else
                        {
                            neighbouringChunks[i, j, k] = new MarchingCubesZerosChunk();
                        }
                    }
                }
            }

            var cas = worldData.chunkAxisSize;
            var crs = worldData.chunkAxsReadoutBlendSize;
            var chunkEdge = (cas - Data.AXIS_READOUT_BLEND_SIZE_OFFSET);
            for (int i = 0; i < crs; i++)
            {
                for (int j = 0; j < crs; j++)
                {
                    for (int k = 0; k < crs; k++)
                    {
                        worldData.pointsArray[i + j * crs + k * crs * crs] =
                            neighbouringChunks[worldData.readoutChunkCArray[i], worldData.readoutChunkCArray[j], worldData.readoutChunkCArray[k]][worldData.readoutChunkDArray[i], worldData.readoutChunkDArray[j], worldData.readoutChunkDArray[k]];
                    }
                }
            }

            return worldData.pointsArray;
        }

#endregion
#region LoadingFunctionality
        public void LoadPointsFromFloatingWorld(FloatingWorldEditable fwe, string seed)
        {
            marchingCubeChunkLoader.worldSeed = seed;
            marchingCubeChunkLoader.worldsGenerated++;

            Init(marchingCubeChunkLoader.worldSeed, marchingCubeChunkLoader.worldsGenerated);

            var lengthVerts = Mathf.Min(worldData.chunkAxisSize * worldData.length, fwe.topHeightValues.GetLength(0));
            var widthVerts = Mathf.Min(worldData.chunkAxisSize * worldData.width, fwe.topHeightValues.GetLength(0));
            for (int x = 0; x < lengthVerts; x++)
            {
                for (int y = 0; y < widthVerts; y++)
                {
                    if (!fwe.isLandValues[x, y])
                    {
                        continue;
                    }

                    int indexX = x % worldData.chunkAxisSize;
                    int chunkX = x / worldData.chunkAxisSize + worldData.edgeThickness;

                    int indexY = y % worldData.chunkAxisSize;
                    int chunkY = y / worldData.chunkAxisSize + worldData.edgeThickness;

                    for (int z = Mathf.CeilToInt(fwe.bottomHeightValues[x, y]); z < fwe.topHeightValues[x, y]; z++)
                    {
                        int indexZ = z % worldData.chunkAxisSize;
                        int chunkZ = z / worldData.chunkAxisSize;

                        if (!marchingCubeChunks[chunkX, chunkY].ContainsKey(chunkZ))
                        {
                            marchingCubeChunks[chunkX, chunkY][chunkZ] = new MarchingCubeChunkData(worldData.chunkAxisSize);
                        }

                        marchingCubeChunks[chunkX, chunkY][chunkZ][indexX, indexY, indexZ] = 2;
                    }
                }
            }
        }

        string Save()
        {
            string mainSave = SaveUtilities.SaveData(marchingCubeChunkLoader);
            for (int i = 0; i < worldData.fullLength; i++)
            {
                for (int j = 0; j < worldData.fullWidth; j++)
                {
                    if (marchingCubeChunks[i, j].Any())
                    {
                        SaveUtilities.SaveData(marchingCubeChunks[i, j]);
                    }
                }
            }
            return mainSave;
        }

        void LoadWorld(string worldPath)
        {
            this.marchingCubeChunkLoader.worldSeed = worldPath.Split('\\').Last();
            SaveUtilities.LoadData(this.marchingCubeChunkLoader);

            Init(this.marchingCubeChunkLoader.worldSeed, this.marchingCubeChunkLoader.worldsGenerated);
            for (int i = 0; i < worldData.fullLength; i++)
            {
                for (int j = 0; j < worldData.fullWidth; j++)
                {
                    SaveUtilities.LoadData(marchingCubeChunks[i, j]);
                }
            }

            RegenerateAllChunks();
        }
#endregion
#region Debug
        public void RegisterDebug()
        {
            string lastPressed = "last pressed";
            string savedPath = "";

            DebugMenu.DebugMenu.Instance.RegisterPanel
                ( "marching cubes"
                , new DebugMenu.DebugOptionAction("regenerate chunks", () => lastPressed, () =>
                    {
                        lastPressed = DateTime.Now.ToString();
                        RegenerateAllChunks();
                    })
                , new DebugOptionAction("Save current world", () => savedPath, () => savedPath = Save())
                );
        }
#endregion
    }
}