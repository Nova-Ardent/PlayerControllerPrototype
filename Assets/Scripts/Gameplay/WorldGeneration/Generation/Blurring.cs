using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public static class Blurring
    {
        public static void Generate(WorldEditable worldEditable, int intensity)
        {
            // can be done with compute shader as well;
            if (intensity == 0)
            {
                return;
            }

            int w = worldEditable.heightMap.GetLength(0);
            int h = worldEditable.heightMap.GetLength(1);
            float[,] blurredMap = new float[w - intensity * 2, h - intensity * 2];

            int wi = w - intensity;
            int hi = h - intensity;
            for (int x = intensity; x < wi; x++)
            {
                for (int y = intensity; y < hi; y++)
                {
                    var xi = x - intensity;
                    var yi = y - intensity;
                    for (int i = -intensity; i <= intensity; i++)
                    {
                        for (int j = -intensity; j <= intensity; j++)
                        {
                            blurredMap[xi, yi] +=
                                worldEditable.heightMap[x + i, y + j];
                        }
                    }
                }
            }

            var sqr = (1 + 2 * intensity) * (1 + 2 * intensity);
            for (int x = intensity; x < wi; x++)
            {
                for (int y = intensity; y < hi; y++)
                {
                    var xi = x - intensity;
                    var yi = y - intensity;
                    worldEditable.heightMap[x, y] = blurredMap[xi, yi] / sqr;
                }
            }
                }
    }
}
