using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using System;

namespace WorldGen
{
    [System.Serializable]
    public class MarchingCubeChunkLoader
        : ISaveable
    {
        [SerializeField] public string worldSeed;
        [SerializeField] public int worldsGenerated;
        [SerializeField] MarchingCubeChunkGenerator marchingCubeChunkGenerator;

        string[] ISaveable.SavePath => new string[] { worldSeed };
        string ISaveable.SaveFile => "worldData.data";
        int ISaveable.Version => 1;

        IEnumerable<byte> ISaveable.GetSavedData()
        {
            byte[] seed = Encoding.ASCII.GetBytes(worldSeed + worldsGenerated);
            byte[] seedLength = BitConverter.GetBytes(seed.Length);

            return seedLength
                .Concat(seed);
        }
    }
}