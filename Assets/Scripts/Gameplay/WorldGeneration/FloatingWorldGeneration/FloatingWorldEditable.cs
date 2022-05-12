using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace WorldGen
{
    public class FloatingWorldEditable
    {
        public bool flatSurfaceShading = true;
        public float[,] topHeightValues;
        public float[,] bottomHeightValues;
        public bool[,] isLandValues;

        public int xTiles;
        public int yTiles;
        public int tileSize;
        float scale;

        public FloatingWorldEditable(int xTiles, int yTiles, int tileSize, float tileScale)
        {
            this.xTiles = xTiles;
            this.yTiles = yTiles;
            this.tileSize = tileSize;
            this.scale = tileScale;

            this.topHeightValues = new float[xTiles * tileSize + 1, yTiles * tileSize + 1];
            this.bottomHeightValues = new float[xTiles * tileSize + 1, yTiles * tileSize + 1];
            this.isLandValues = new bool[xTiles * tileSize + 1, yTiles * tileSize + 1];
        }

        public void TranslateTop(float amount)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    topHeightValues[x, y] += amount;
                }
            }
        }

        public void TranslateTopByPercent(float amount, int iterations, int edgeThickness)
        {
            float maxValue = -1;
            float minValue = float.MaxValue;
            
            int w = bottomHeightValues.GetLength(0);
            int h = bottomHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (bottomHeightValues[x, y] < minValue)
                    {
                        minValue = bottomHeightValues[x, y];
                    }
                }
            }

            for (int i = 0; i < iterations; i++)
            {
                float percentageOfLand = 0;
                for (int x = 1; x < w - 1; x++)
                {
                    for (int y = 1; y < h - 1; y++)
                    {
                        percentageOfLand += topHeightValues[x, y] - edgeThickness > bottomHeightValues[x, y] ? 1 : 0;
                    }
                }

                percentageOfLand /= (w - 2) * (h - 2);
                float amountOffset = (amount / 100 - percentageOfLand) * (maxValue - minValue) / 2;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        topHeightValues[x, y] += amountOffset;
                    }
                }
            }
        }

        public void TranslateBottom(float amount)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    topHeightValues[x, y] += amount;
                }
            }
        }

        public void SetBooleanInterceptValues(int edgeThickness)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    isLandValues[x, y] = true;
                    isLandValues[x, y] = topHeightValues[x, y] - edgeThickness > bottomHeightValues[x, y];
                }
            }
        }

        public void RemovePoorNeighbours()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    int check = 0;
                    if (!isLandValues[x, y])
                    {
                        continue;
                    }

                    if (isLandValues[x + 1, y]) check++;
                    if (isLandValues[x + 1, y + 1]) check++;
                    if (isLandValues[x + 1, y - 1]) check++;
                    if (isLandValues[x - 1, y]) check++;
                    if (isLandValues[x - 1, y + 1]) check++;
                    if (isLandValues[x - 1, y - 1]) check++;
                    if (isLandValues[x, y + 1]) check++;
                    if (isLandValues[x, y - 1]) check++;

                    isLandValues[x, y] = check > 3;
                }
            }
        }

        public void MirrorOffsetTerrain()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);

            float sumOfPoints = 0;
            for (int x = 1; x < w; x++)
            {
                for (int y = 1; y < h; y++)
                {
                    sumOfPoints += topHeightValues[x, y] > 0 ? 1 : -1;
                }
            }

            if (sumOfPoints < 0)
            {
                for (int x = 1; x < w; x++)
                {
                    for (int y = 1; y < h; y++)
                    {
                        topHeightValues[x, y] *= -1;
                    }
                }
            }

            sumOfPoints = 0;
            for (int x = 1; x < w; x++)
            {
                for (int y = 1; y < h; y++)
                {
                    sumOfPoints += bottomHeightValues[x, y] > 0 ? 1 : -1;
                }
            }

            if (sumOfPoints > 0)
            {
                for (int x = 1; x < w; x++)
                {
                    for (int y = 1; y < h; y++)
                    {
                        bottomHeightValues[x, y] *= -1;
                    }
                }
            }
        }

        public void MergeEdges()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (!isLandValues[x, y])
                    {
                        continue;
                    }

                    LODHeightEdgeAdjust(x, y, 1, out topHeightValues[x, y], out bottomHeightValues[x, y]);
                }
            }
        }

        public void TranslateAboveZero()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);

            float min = 0;
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    min = Math.Min(bottomHeightValues[x, y], min);
                }
            }

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    topHeightValues[x, y] -= min;
                    bottomHeightValues[x, y] -= min;
                }
            }
        }

        public void LODHeightEdgeAdjust(int x, int y, int step, out float top, out float bottom)
        {
            if (isLandValues[x, y] && (
                !isLandValues[x + step, y] ||
                !isLandValues[x + step, y + step] ||
                !isLandValues[x + step, y - step] ||
                !isLandValues[x - step, y] ||
                !isLandValues[x - step, y + step] ||
                !isLandValues[x - step, y - step] ||
                !isLandValues[x, y + step] ||
                !isLandValues[x, y - step]))
            {
                top = (topHeightValues[x, y] + bottomHeightValues[x, y]) / 2;
                bottom = top;
                return;
            }

            top = topHeightValues[x, y];
            bottom = bottomHeightValues[x, y];
        }
    }
}