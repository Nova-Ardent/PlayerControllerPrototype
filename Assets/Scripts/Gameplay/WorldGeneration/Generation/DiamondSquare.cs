using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WorldGen
{
    public static class DiamondSquare
    {
        public static void Generate(WorldEditable worldEditable, float rougness, System.Random random, bool zerodEdge = false)
        {
            int dataSize = Mathf.Min(worldEditable.heightMap.GetLength(0), worldEditable.heightMap.GetLength(1)) - 1;
            if (dataSize < 4)
            {
                Debug.LogError($"{dataSize} is too small.");
                return;
            }

            dataSize = (int)Mathf.Pow(2, (int)Mathf.Log(dataSize, 2)) + 1;
            worldEditable.heightMap[0, 0] = 0;
            worldEditable.heightMap[0, dataSize - 1] = 0;
            worldEditable.heightMap[dataSize - 1, 0] = 0;
            worldEditable.heightMap[dataSize - 1, dataSize - 1] = 0;

            for (int sideLength = dataSize - 1;
                sideLength >= 2;
                sideLength /= 2, rougness /= 2.0f)
            {
                int halfSide = sideLength / 2;

                for (int x = 0; x < dataSize - 1; x += sideLength)
                {
                    for (int y = 0; y < dataSize - 1; y += sideLength)
                    {
                        float avg = worldEditable.heightMap[x, y] +
                        worldEditable.heightMap[x + sideLength, y] +
                        worldEditable.heightMap[x, y + sideLength] +
                        worldEditable.heightMap[x + sideLength, y + sideLength];
                        avg /= 4.0f;

                        var xStep = x + halfSide;
                        var yStep = y + halfSide;
                        if (xStep == 0 || yStep == 0 || xStep == dataSize - 1 || yStep == dataSize -1)
                        {
                            worldEditable.heightMap[xStep, yStep] = 0;
                        }
                        else
                        {
                            worldEditable.heightMap[xStep, yStep] =
                                avg + ((float)random.NextDouble() * 2 * rougness) - rougness;
                        }
                    }
                }

                for (int x = 0; x < dataSize - 1; x += halfSide)
                {
                    for (int y = (x + halfSide) % sideLength; y < dataSize - 1; y += sideLength)
                    {
                        float avg =
                          worldEditable.heightMap[(x - halfSide + dataSize) % dataSize, y] +
                          worldEditable.heightMap[(x + halfSide) % dataSize, y] +
                          worldEditable.heightMap[x, (y + halfSide) % dataSize] +
                          worldEditable.heightMap[x, (y - halfSide + dataSize) % dataSize];
                        avg /= 4.0f;

                        avg = avg + ((float)random.NextDouble() * 2 * rougness) - rougness;

                        if (x == 0 || y == 0 || y == dataSize - 1 || y == dataSize - 1)
                        {
                            worldEditable.heightMap[x, y] = 0;
                        }
                        else
                        {
                            worldEditable.heightMap[x, y] = avg;
                        }

                        if (x == 0)
                        {
                            worldEditable.heightMap[dataSize - 1, y] = 0;
                        }
                        if (y == 0)
                        {
                            worldEditable.heightMap[x, dataSize - 1] = 0;
                        }
                    }
                }
            }
        }
    }
}
