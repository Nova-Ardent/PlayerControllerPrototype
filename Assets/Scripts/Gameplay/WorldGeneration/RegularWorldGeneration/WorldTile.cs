using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldGen
{
    public class WorldTile : MonoBehaviour
    {
        [System.Serializable]
        public struct CustomLODDistancing
        {
            public bool Active;
            public float LOD0Distance;
            public float LOD1Distance;
            public float LOD2Distance;
            public float LOD3Distance;
            public float LOD4Distance;

            [System.NonSerialized] public float LOD0MagnitudeValue;
            [System.NonSerialized] public float LOD1MagnitudeValue;
            [System.NonSerialized] public float LOD2MagnitudeValue;
            [System.NonSerialized] public float LOD3MagnitudeValue;
            [System.NonSerialized] public float LOD4MagnitudeValue;
        }

        [SerializeField] LODGroup lodGroup;

        public CustomLODDistancing customLODDistancing;
        public float scale;
        public int xSegments;
        public int ySegments;

        public Vector2 tileCenter;
        int tempX;
        int tempY;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void DisableLOD()
        {
            lodGroup.ForceLOD(0);
        }

        public void EnableLOD()
        {
            lodGroup.ForceLOD(-1);
        }

        public void SetLOD(int i)
        {
            lodGroup.ForceLOD(i);
        }

        public void ApplyLOD(int index, params GameObject[] meshObject)
        {
            if (index == 0)
            {
                //var collider = this.gameObject.AddComponent<MeshCollider>();
                //collider.sharedMesh = meshObject.GetComponent<MeshFilter>().mesh;
            }

            for (int i = 0; i < meshObject.Length; i++)
            {
                meshObject[i].transform.SetParent(transform);
            }

            var lods = lodGroup.GetLODs();
            lods[index].renderers = lods[index].renderers.Concat(meshObject.Select(x => x.GetComponent<Renderer>())).ToArray();
            lodGroup.SetLODs(lods);
        }
    }
}
