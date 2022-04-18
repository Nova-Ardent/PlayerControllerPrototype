using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    [System.Serializable]
    public class SinWaveIntensity
    {
        [System.Serializable]
        public struct SinWave
        {
            float scale;
            float frequencyMin;
            float frequencyMax;
            float offsetMin;
            float offsetMax;
        }

        SinWave[] sinWaves = new SinWave[0];
    }

    public static class SinIntensity
    {
        public static void Generate(WorldEditable worldGen, SinWaveIntensity sinWaveIntensity, System.Random random)
        {

        }
    }
}