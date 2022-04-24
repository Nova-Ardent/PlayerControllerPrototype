using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WorldGen
{
    public static class DiamondSquare
    {
        public static float[,] Generate(float[,] heightMap, float rougness, System.Random random)
        {
            int dataSize = Mathf.Min(heightMap.GetLength(0), heightMap.GetLength(1)) - 1;
            if (dataSize < 4)
            {
                Debug.LogError($"{dataSize} is too small.");
                return heightMap;
            }

            dataSize = (int)Mathf.Pow(2, (int)Mathf.Log(dataSize, 2)) + 1;
            heightMap[0, 0] = 0;
            heightMap[0, dataSize - 1] = 0;
            heightMap[dataSize - 1, 0] = 0;
            heightMap[dataSize - 1, dataSize - 1] = 0;

            for (int sideLength = dataSize - 1;
                sideLength >= 2;
                sideLength /= 2, rougness /= 2.0f)
            {
                int halfSide = sideLength / 2;

                for (int x = 0; x < dataSize - 1; x += sideLength)
                {
                    for (int y = 0; y < dataSize - 1; y += sideLength)
                    {
                        float avg = heightMap[x, y] +
                        heightMap[x + sideLength, y] +
                        heightMap[x, y + sideLength] +
                        heightMap[x + sideLength, y + sideLength];
                        avg /= 4.0f;

                        var xStep = x + halfSide;
                        var yStep = y + halfSide;
                        if (xStep == 0 || yStep == 0 || xStep == dataSize - 1 || yStep == dataSize -1)
                        {
                            heightMap[xStep, yStep] = 0;
                        }
                        else
                        {
                            heightMap[xStep, yStep] =
                                avg + ((float)random.NextDouble() * 2 * rougness) - rougness;
                        }
                    }
                }

                for (int x = 0; x < dataSize - 1; x += halfSide)
                {
                    for (int y = (x + halfSide) % sideLength; y < dataSize - 1; y += sideLength)
                    {
                        float avg =
                          heightMap[(x - halfSide + dataSize) % dataSize, y] +
                          heightMap[(x + halfSide) % dataSize, y] +
                          heightMap[x, (y + halfSide) % dataSize] +
                          heightMap[x, (y - halfSide + dataSize) % dataSize];
                        avg /= 4.0f;

                        avg = avg + ((float)random.NextDouble() * 2 * rougness) - rougness;

                        if (x == 0 || y == 0 || y == dataSize - 1 || y == dataSize - 1)
                        {
                            heightMap[x, y] = 0;
                        }
                        else
                        {
                            heightMap[x, y] = avg;
                        }

                        if (x == 0)
                        {
                            heightMap[dataSize - 1, y] = 0;
                        }
                        if (y == 0)
                        {
                            heightMap[x, dataSize - 1] = 0;
                        }
                    }
                }
            }

            return heightMap;
        }
    }
}
