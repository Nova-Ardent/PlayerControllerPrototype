using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class MarchingCubeChunkData : MarchingCubesChunkBase
    {
        public override float this[int x, int y, int z]
        {
            get => points[z * zOffset + y * yOffset + x];
            set => points[z * zOffset + y * yOffset + x] = value;
        }

        int scale;
        int yOffset;
        int zOffset;

        public MarchingCubeChunkData(int scale)
        {
            this.scale = scale;
            this.yOffset = scale;
            this.zOffset = scale * scale;
            points = new float[scale * scale * scale];
        }
    }
}
