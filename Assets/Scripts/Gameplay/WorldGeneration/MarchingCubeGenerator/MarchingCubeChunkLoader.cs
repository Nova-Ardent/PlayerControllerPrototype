using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using System;
using System.IO;

namespace WorldGen
{
    [System.Serializable]
    public class MarchingCubeChunkLoader
        : ISaveable
    {
        bool ISaveable.IgnoreFileNotFound { get => false; }

        enum LoadingState
        {
            StartOfStream,
            NameSize,
            Name,
            WorldsGenerated,
            ChunkLength,
            ChunkWidth,
            ChunkAxisSize,
            ISO,
            Done,
        }

        LoadingState loadingState = LoadingState.StartOfStream;

        [SerializeField] public string worldSeed;
        [SerializeField] public int worldsGenerated;
        [SerializeField] MarchingCubeChunkGenerator marchingCubeChunkGenerator;

        string[] ISaveable.SavePath => new string[] { worldSeed };
        string ISaveable.SaveFile => "worldData.data";
        int ISaveable.Version => 1;

        IEnumerable<byte> ISaveable.GetSavedData()
        {
            byte[] seed = Encoding.ASCII.GetBytes(worldSeed);
            byte[] seedLength = BitConverter.GetBytes(seed.Length);

            byte[] worldsGenerated = BitConverter.GetBytes(this.worldsGenerated);
            byte[] chunkLen = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.length);
            byte[] chunkWid = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.width);
            byte[] chunkAxisSize = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.chunkAxisSize);
            byte[] iso = BitConverter.GetBytes(marchingCubeChunkGenerator.worldData.iso);

            return seedLength
                .Concat(seed)
                .Concat(worldsGenerated)
                .Concat(chunkLen)
                .Concat(chunkWid)
                .Concat(chunkAxisSize)
                .Concat(iso);
        }

        int ISaveable.SetSavedData(byte[] nextBytes, int version)
        {
            switch (loadingState)
            {
                case LoadingState.StartOfStream:
                    loadingState++;
                    return sizeof(int);
                case LoadingState.NameSize:
                    loadingState++;
                    return BitConverter.ToInt32(nextBytes, 0);
                case LoadingState.Name:
                    loadingState++;
                    worldSeed = Encoding.ASCII.GetString(nextBytes);
                    return sizeof(int);
                case LoadingState.WorldsGenerated:
                    loadingState++;
                    worldsGenerated = BitConverter.ToInt32(nextBytes, 0);
                    return sizeof(int);
                case LoadingState.ChunkLength:
                    loadingState++;
                    marchingCubeChunkGenerator.worldData.length = BitConverter.ToInt32(nextBytes, 0);
                    return sizeof(int);
                case LoadingState.ChunkWidth:
                    loadingState++;
                    marchingCubeChunkGenerator.worldData.width = BitConverter.ToInt32(nextBytes, 0);
                    return sizeof(int);
                case LoadingState.ChunkAxisSize:
                    loadingState++;
                    marchingCubeChunkGenerator.worldData.chunkAxisSize = BitConverter.ToInt32(nextBytes, 0);
                    return sizeof(float);
                case LoadingState.ISO:
                    loadingState++;
                    marchingCubeChunkGenerator.worldData.iso = BitConverter.ToSingle(nextBytes, 0);
                    return 0;
                case LoadingState.Done:
                    loadingState = LoadingState.StartOfStream;
                    return 0;
                default:
                    return 0;
            }
        }

        void ISaveable.OnStreamEnd()
        {
            if (loadingState != LoadingState.Done)
            {
                Debug.LogError("hit the end of file stream without fully setting all the data.");
                return;
            }

            loadingState = LoadingState.StartOfStream;
        }

        public static List<string> GetWorldFolders()
        {
            List<string> paths = new List<string>();
            var basePath = Application.persistentDataPath;
            foreach (var path in Directory.GetDirectories(basePath))
            {
                if (File.Exists(Path.Combine(path, "worldData.data")))
                {
                    paths.Add(path);
                }
            }
            return paths;
        }
    }
}