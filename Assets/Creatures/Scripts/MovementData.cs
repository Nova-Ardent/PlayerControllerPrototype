using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Creatures/MovementData/MovementData")]
    public class MovementData : ScriptableObject
    {
        [Header("Movement")]
        public float WalkingSpeed;
        public float RunningSpeed;
        public float MovementTransitionTime;

        [Header("Gravity")]
        public float Gravity = 9.81f;
        public float DeathFallingSpeed = -15f;

        [Header("Grounded")]
        public LayerMask GroundLayers;
        public Vector3 GroundedPosition;
        public float GroundedRadius;
        public Color GroundedDrawColor;
        public Color NotGroundedDrawColor;

        [Header("Jumping")]
        public float JumpAtNormalized;
        public float MovingJumpAtNormalized;
        public float JumpDelay;
        public float JumpStrength;

        [Header("Character Snapping")]
        [Range(0, 1)] public float RotationSpeed;  
    }
}
