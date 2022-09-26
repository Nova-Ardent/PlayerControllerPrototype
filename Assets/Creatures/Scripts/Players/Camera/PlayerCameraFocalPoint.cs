using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Camera
{
    [CreateAssetMenu(fileName = "Focal Point", menuName = "Creatures/Camera/CameraFocalPoint")]
    public class PlayerCameraFocalPoint : ScriptableObject
    {
        [SerializeField] public Vector3 CameraArmLength;
        [SerializeField] public Vector3 RelativeAngleToFocus;
        [SerializeField] public float RotationSnapSpeed;
    }
}
