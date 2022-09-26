#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class TestingSelectorRing : MonoBehaviour
    {
        [SerializeField] bool goInReverse;
        [SerializeField] SelectorRing selectorRing;
        float incrementTimer = 0;

        private void Update()
        {
            incrementTimer -= Time.deltaTime;
            if (incrementTimer < 0)
            {
                incrementTimer = 1;
                if (goInReverse)
                {
                    selectorRing.Decrement();
                }
                else
                {
                    selectorRing.Increment();
                }
            }
            
            selectorRing.UpdateRotation();
        }

    }
}

#endif