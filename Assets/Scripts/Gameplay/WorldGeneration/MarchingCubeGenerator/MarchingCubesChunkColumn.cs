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
        [SerializeField] public string worldSeed;
        [SerializeField] public int worldsGenerated;
        [SerializeField] int x;
        [SerializeField] int y;

        string[] ISaveable.SavePath => new string[] { worldSeed, $"gen{worldsGenerated}" };

        string ISaveable.SaveFile => $"chunk{x}_{y}.data";

        int ISaveable.Version => 1;

        IEnumerable<byte> ISaveable.GetSavedData()
        {
            foreach (var key in Keys)
            {
                foreach (var keyBits in BitConverter.GetBytes(key))
                {
                    yield return keyBits;
                }

                var chunk = this[key];
                if (chunk.points.Length == 0)
                {
                    Debug.LogError("attempted to save an empty chunk.");
                    continue;
                }

                foreach (var chunkSizeBits in BitConverter.GetBytes(chunk.points.Length))
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
            return 0;
        }

        public MarchingCubesChunkColumn(string seed, int worldGenerated, int x, int y)
            : base()
        {
            this.worldSeed = seed;
            this.worldsGenerated = worldGenerated;
            this.x = x;
            this.y = y;
        }
    }
}
