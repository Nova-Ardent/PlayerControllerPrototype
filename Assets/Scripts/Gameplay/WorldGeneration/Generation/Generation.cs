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
            WorldGen.DiamondSquare.Generate(worldEditable, roughness, random);
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
