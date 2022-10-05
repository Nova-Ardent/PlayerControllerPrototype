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
        [Serializable]
        public class TrendTowardsData
        {
            public enum TrendType
            {
                Single,
                Curve,
            }

            public TrendType trendType;
            public Vector3 direction;
            
            public float fit;
            public AnimationCurve curveFit;
        }

        public enum GenerationMethod
        {
            Random,
            Trend,
        }

        [SerializeField] public GenerationMethod generationMethod;
        [Range(0, 80f), SerializeField] public float branchAngle = 90;

        public TrendTowardsData trendTowardsData;

        public float BranchAngle
        {
            get => branchAngle;
        }

        public Vector3 GeneratePoint(Vector3 from, Vector3 to, System.Random random, int currentPoint, int count)
        {
            switch (generationMethod)
            {
                case GenerationMethod.Random:
                    return GenerateRandomPoint(from, to, random);
                case GenerationMethod.Trend:
                    return GenerateTrendingPoint(from, to, random, currentPoint, count);
                default:
                    Debug.LogError("undefined point generation method");
                    return GenerateRandomPoint(from, to, random);
            }
        }

        public Vector3 GenerateRandomPoint(Vector3 from, Vector3 to, System.Random random, float intensity = 1)
        {
            float radius = Vector3.Distance(from, to) * Mathf.Tan(branchAngle) / 4;
            Quaternion coneDirection = Quaternion.FromToRotation(Vector3.up, from - to);

            Vector2 position = Utilities.Utilities.PolarToCartesian(
                (float)random.NextDouble() * radius * intensity,
                (float)random.NextDouble() * 2 * Mathf.PI);
            return coneDirection * new Vector3(
                position.x,
                0,
                position.y) + to;
        }

        public Vector3 GenerateTrendingPoint(Vector3 from, Vector3 to, System.Random random, int currentPoint, int count)
        {
            Debug.Assert(trendTowardsData != null);

            float percentage = ((float)currentPoint) / count;
            float fit = 0;
            switch (trendTowardsData.trendType)
            {
                case TrendTowardsData.TrendType.Single:
                    fit = trendTowardsData.fit;              
                    break;
                case TrendTowardsData.TrendType.Curve:
                    fit = trendTowardsData.curveFit.Evaluate(percentage);
                    break;
                default:
                    Debug.LogError("undefined trending type.");
                    return Vector3.zero;
            }

            to = Vector3.LerpUnclamped(to - from, trendTowardsData.direction, fit).normalized + from;
            return GenerateRandomPoint(from, to, random, 1 - Mathf.Clamp(fit, 0, 1));
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(VectorTreeAngle))]
    public class VectorTreeAnglePropertyDrawer : PropertyDrawerBuilder<VectorTreeAngle>
    {
        Foldout initialFoldout = new Foldout();
        Foldout trendFoldout = new Foldout();

        public override void GenerateGUI()
        {
            if (!OnFoldout(ref initialFoldout))
            {
                return;
            }

            using (Depth())
            {
                Header("General Data");
                currentData.generationMethod = EnumOption("branching method", currentData.generationMethod);
                currentData.branchAngle = SliderOption("branch angle", currentData.branchAngle, 0, 80f, 100);

                switch (currentData.generationMethod)
                {
                    case VectorTreeAngle.GenerationMethod.Random:
                        break;
                    case VectorTreeAngle.GenerationMethod.Trend:
                        TrendTowards();
                        break;
                }
            }
        }

        public void TrendTowards()
        {
            Space();
            Header("Trend");
            
            if (!OnFoldout(ref trendFoldout))
            {
                return;
            }

            using (Depth())
            {
                currentData.trendTowardsData.trendType = EnumOption("trend type", currentData.trendTowardsData.trendType);
                currentData.trendTowardsData.direction = Vector3Option("direction", currentData.trendTowardsData.direction);

                if (currentData.trendTowardsData.trendType == VectorTreeAngle.TrendTowardsData.TrendType.Single)
                {
                    currentData.trendTowardsData.fit = SliderOption("fit", currentData.trendTowardsData.fit, -1, 1, 100);
                    currentData.trendTowardsData.curveFit = null;
                }
                else
                {
                    if (currentData.trendTowardsData.curveFit == null)
                    {
                        currentData.trendTowardsData.curveFit = AnimationCurve.Linear(0, 0, 1, 0);
                    }
                    currentData.trendTowardsData.curveFit = CurveOption(
                        "fit curve",
                        currentData.trendTowardsData.curveFit,
                        new Rect(0, -1, 1, 2),
                        graphHeight: 100);
                }
            }
        }
    }
#endif
}
