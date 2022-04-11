using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public static class AITesting
    {
        public static void Generate(WorldEditable worldEditable)
        {
            for (int i = 0; i < 100; i++)
            {
                worldEditable.heightMap[20 + i, 20] = i / 4.0f;
                worldEditable.heightMap[20 + i, 21] = i / 4.0f;
            }
        }
    }
}
