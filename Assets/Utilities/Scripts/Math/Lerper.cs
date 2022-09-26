using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Math
{
    public class Lerper
    {
        float current = 0;

        readonly float lerpScale;

        public float Value
        {
            get => current;
        }

        public Lerper(float lerpScale)
        {
            this.lerpScale = lerpScale;
        }

        public void Reset()
        {
            current = 0;
        }

        public bool Update()
        {
            if (Value == 1)
            {
                return true;
            }

            current = Mathf.Min(current + Time.deltaTime / lerpScale, 1);
            return false;
        }
    }
}
