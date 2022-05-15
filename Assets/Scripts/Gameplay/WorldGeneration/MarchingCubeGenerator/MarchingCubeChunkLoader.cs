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

            byte[] chunkLen = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.length);
            byte[] chunkWid = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.width);
            byte[] chunkAxisSize = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.chunkAxisSize);
            byte[] iso = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.iso);

            return seedLength
                .Concat(seed)
                .Concat(chunkLen)
                .Concat(chunkWid)
                .Concat(chunkAxisSize)
                .Concat(iso);
        }

        int ISaveable.SetSavedData(byte[] nextBytes, int version)
        {
            return 0;
        }
    }
}