using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Utilities.Unity;
using System.IO;
using System.Runtime.CompilerServices;

namespace Procedural.Builders
{
    [Serializable]
    public class VectorTreeAngle
    {
        public enum GenerationMethod
        {
            Random,
            TrendToward,
        }

#region General
        [SerializeField] public GenerationMethod generationMethod;
        [Range(0, 80f), SerializeField] public float branchAngle = 90;
#endregion


        public float BranchAngle
        {
            get => branchAngle;
        }

        public Vector3 GeneratePoint(Vector3 from, Vector3 to, System.Random random)
        {
            float radius = Vector3.Distance(from, to) * Mathf.Tan(branchAngle) / 2;
            Quaternion coneDirection = Quaternion.FromToRotation(Vector3.up, from - to);

            Vector2 position = Utilities.Utilities.PolarToCartesian(
                (float)random.NextDouble() * radius,
                (float)random.NextDouble() * 2 * Mathf.PI);
            return coneDirection * new Vector3(
                position.x,
                0,
                position.y) + to;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(VectorTreeAngle))]
    public class VectorTreeAnglePropertyDrawer : PropertyDrawerBuilder<VectorTreeAngle>
    {
        Foldout initialFoldout = new Foldout();

        public override void GenerateGUI()
        {
            if (OnFoldout(ref initialFoldout))
            {
                using (Depth())
                {
                    Header("General Data");
                    currentData.generationMethod = EnumOption("branching method", currentData.generationMethod);
                    currentData.branchAngle = Slider("branch angle", currentData.branchAngle, 0, 80f, 100);
                    Space();
                    Header("Trend towards");
                }

                return;
            }
        }
    }
#endif
}
