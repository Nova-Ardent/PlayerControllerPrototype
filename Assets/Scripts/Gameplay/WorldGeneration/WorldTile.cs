using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldGen
{
    public class WorldTile : MonoBehaviour
    {
        [SerializeField] LODGroup lodGroup;

        public float scale;
        public int xSegments;
        public int ySegments;

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

        public void ApplyLOD(GameObject meshObject, int index)
        {
            if (index == 0)
            {
                var collider = this.gameObject.AddComponent<MeshCollider>();
                collider.sharedMesh = meshObject.GetComponent<MeshFilter>().mesh;
            }

            meshObject.transform.SetParent(transform);

            var lods = lodGroup.GetLODs();
            lods[index].renderers = lods[index].renderers.Append(meshObject.GetComponent<Renderer>()).ToArray();
            lodGroup.SetLODs(lods);
        }
    }
}
