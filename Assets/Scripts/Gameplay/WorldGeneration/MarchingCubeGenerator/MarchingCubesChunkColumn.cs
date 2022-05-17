using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using System;

namespace WorldGen
{
    public class MarchingCubesChunkColumn
        : Dictionary<int, MarchingCubesChunkBase>
        , ISaveable
    {
        bool ISaveable.IgnoreFileNotFound { get => true; }

        enum LoadingState
        {
            StartOfStream,
            GetKey,
            GetSize,
            GetPoints,
        }

        class LoadingHelper
        {
            public LoadingState loadingState = LoadingState.StartOfStream;
            public int lastKeyRead = 0;
            public int lastSizeRead = 0;
        }


        LoadingHelper loadingHelper = new LoadingHelper();

        [SerializeField] public string worldSeed;
        [SerializeField] public int worldsGenerated;
        [SerializeField] int x;
        [SerializeField] int y;

        string[] ISaveable.SavePath => new string[] { worldSeed, $"gen{worldsGenerated}" };

        string ISaveable.SaveFile => $"chunk{x}_{y}.data";

        int ISaveable.Version => 1;

        public MarchingCubesChunkColumn(string seed, int worldGenerated, int x, int y)
            : base()
        {
            this.worldSeed = seed;
            this.worldsGenerated = worldGenerated;
            this.x = x;
            this.y = y;
        }

        IEnumerable<byte> ISaveable.GetSavedData()
        {
            foreach (var key in Keys)
            {
                foreach (var keyBits in BitConverter.GetBytes(key))
                {
                    yield return keyBits;
                }

                var chunk = this[key];
                if (chunk.scale == 0)
                {
                    Debug.LogError("attempted to save an empty chunk.");
                    continue;
                }

                foreach (var chunkSizeBits in BitConverter.GetBytes(chunk.scale))
                {
                    yield return chunkSizeBits;
                }

                for (int i = 0; i < chunk.points.Length; i++)
                {
                    foreach (var point in BitConverter.GetBytes(chunk.points[i]))
                    {
                        yield return point;
                    }
                }
            }
        }

        int ISaveable.SetSavedData(byte[] nextBytes, int version)
        {
            switch (loadingHelper.loadingState)
            {
                case LoadingState.StartOfStream:
                    loadingHelper.loadingState++;
                    return sizeof(int);
                case LoadingState.GetKey:
                    loadingHelper.loadingState++;
                    loadingHelper.lastKeyRead = BitConverter.ToInt32(nextBytes, 0);
                    return sizeof(int);
                case LoadingState.GetSize:
                    loadingHelper.loadingState++;
                    loadingHelper.lastSizeRead = BitConverter.ToInt32(nextBytes, 0);
                    return loadingHelper.lastSizeRead * loadingHelper.lastSizeRead * loadingHelper.lastSizeRead * sizeof(float);
                case LoadingState.GetPoints:
                    loadingHelper.loadingState = LoadingState.GetKey;
                    var chunk = new MarchingCubeChunkData(loadingHelper.lastSizeRead);
                    for (int i = 0; i < nextBytes.Length; i += sizeof(float))
                    {
                        float p = BitConverter.ToSingle(nextBytes, i);
                        chunk.points[i / sizeof(float)] = p;
                    }
                    this[loadingHelper.lastKeyRead] = chunk;
                    return sizeof(int);
                default:
                    return 0;
            }

        }

        void ISaveable.OnStreamEnd()
        {
            loadingHelper.loadingState = LoadingState.StartOfStream;
        }
    }
}
