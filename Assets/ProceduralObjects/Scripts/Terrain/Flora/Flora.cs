using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Procedural.Builders;

namespace Procedural.Terrain.Plant
{
    public class Flora : MonoBehaviour
    {
        public VectorTree spline;

        // Start is called before the first frame update
        void Start()
        {
            spline.Build();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmosSelected()
        {
            spline.DrawNodes(transform);
        }
    }
}
