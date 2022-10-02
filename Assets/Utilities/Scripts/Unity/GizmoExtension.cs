using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Unity
{
    public static class GizmoExtension
    {
        public static void DrawConeDegrees(Vector3 point, Vector3 baseCenter, float angle, int definition = 16)
        {
            DrawCone(point, baseCenter, Utilities.DegreesToRads(angle), definition);
        }

        public static void DrawCone(Vector3 point, Vector3 baseCenter, float angle, int definition = 16)
        {
            float radius = Vector3.Distance(point, baseCenter) * Mathf.Tan(angle);
            Quaternion coneDirection = Quaternion.FromToRotation(Vector3.up, point - baseCenter);

            // base circle
            Vector3 previousPosition = Vector3.right * radius;
            previousPosition = coneDirection * previousPosition;
            previousPosition += baseCenter;
            for (int i = 0; i < definition + 1; i++)
            {
                Vector3 position = new Vector3(
                    radius * Mathf.Cos(2 * Mathf.PI * i / definition),
                    0,
                    radius * Mathf.Sin(2 * Mathf.PI * i / definition));
                position = coneDirection * position;
                position += baseCenter;

                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }

            // cone
            for (int i = 0; i < 8; i++)
            {
                Vector3 position = new Vector3(
                    radius * Mathf.Cos(2 * Mathf.PI * i / 8),
                    0,
                    radius * Mathf.Sin(2 * Mathf.PI * i / 8));
                position = coneDirection * position;
                position += baseCenter;

                Gizmos.DrawLine(point, position);
            }
        }
    }
}
