using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;

namespace WorldGen
{
    public static class Generation
    {
        static System.Random random = new System.Random(); 

        public static void SetSeed(string seed)
        {
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(seed));

            int seedInt = BitConverter.ToInt32(hashed, 0)
                ^ BitConverter.ToInt32(hashed, 4)
                ^ BitConverter.ToInt32(hashed, 8)
                ^ BitConverter.ToInt32(hashed, 12);
            random = new System.Random(seedInt);
        }

        public static void DiamondSquare(WorldEditable worldEditable, float roughness)
        {
            worldEditable.heightMap = WorldGen.DiamondSquare.Generate(worldEditable.heightMap, roughness, random);
        }

        public static void DiamondSquare(FloatingWorldEditable floatingWorldEditable, bool useTop, float topRoughness, bool useBottom, float bottomRoughness)
        {
            if (useTop)
            {
                floatingWorldEditable.topHeightValues = WorldGen.DiamondSquare.Generate(floatingWorldEditable.topHeightValues, topRoughness, random);
            }

            if (useBottom)
            {
                floatingWorldEditable.bottomHeightValues = WorldGen.DiamondSquare.Generate(floatingWorldEditable.bottomHeightValues, bottomRoughness, random);
            }
        }

        public static void Blurring(WorldEditable worldEditable, int intensity)
        {
            WorldGen.Blurring.Generate(worldEditable, intensity);
        }

        public static void AITesting(WorldEditable worldEditable)
        {
            WorldGen.AITesting.Generate(worldEditable);
        }

        public static void Erosion(WorldEditable worldEditable, ErosionData erosionData)
        {
            WorldGen.Erosion.Generate(worldEditable, erosionData, random);
        }

        public static void SinWaveIntensity(WorldEditable worldEditable, SinWaveIntensity sinWaveIntensity)
        {
            WorldGen.SinIntensity.Generate(worldEditable, sinWaveIntensity, random);
        }
    }
}
