using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    [System.Serializable]
    public struct WorldData
    {
        public int tiles;

        public int tileSize;
        public float tileScale;

        public bool LODActive;
        public bool verbos;
        public string seed;
        public bool AITestingWorld;
    }
}
