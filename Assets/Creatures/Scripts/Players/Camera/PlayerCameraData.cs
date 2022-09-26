using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Camera
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Creatures/Camera/CameraData")]
    public class PlayerCameraData : ScriptableObject
    {
        [Header("Camera Position")]
        public Vector3 CameraArmLength;
        public Vector3 CameraAngle;

        [Header("Camera Speeds")]
        [Range(0, 1f)] public float PositionSnapSpeed;
        [Range(0, 1f)] public float RotationSnapSpeed;
        [Range(0, 1000f)] public float RotationSpeedX;
        [Range(0, 1000f)] public float RotationSpeedY;

        [Header("Camera Limitation")]
        public float MinYRotation;
        public float MaxYRotation;

        [Header("Cursor")]
        public CursorLockMode CursorLocked;
    }
}
