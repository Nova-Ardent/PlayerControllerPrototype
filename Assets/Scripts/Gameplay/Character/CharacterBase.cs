using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CharacterController))]
public class CharacterBase : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string name;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [NonSerialized] public float currentSpeed = 0;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [NonSerialized] public bool isAlive = true;

        // player
        [NonSerialized] public float _speed;
        [NonSerialized] public float _targetRotation = 0.0f;
        [NonSerialized] public float _rotationVelocity;
        [NonSerialized] public float _verticalVelocity;
        [NonSerialized] public float _terminalVelocity = 53.0f;

        // timeout deltatime
        [NonSerialized] public float _jumpTimeoutDelta;
        [NonSerialized] public float _fallTimeoutDelta;
    }

    [System.Serializable]
    public class AminationData
    {
        public Animator animator;

        [NonSerialized] public int walkingHash;
        [NonSerialized] public int walkingSpeedHash;
        [NonSerialized] public int runningHash;
        [NonSerialized] public int jumpHash;
        [NonSerialized] public int groundedHash;
        [NonSerialized] public int fallingSpeedHash;
        [NonSerialized] public int lockedOnHash;
        [NonSerialized] public int walkingDirectionHash; // only important when locked on

        bool _walking;
        public bool walking
        {
            get => _walking;
            set
            {
                if (value != _walking)
                {
                    _walking = value;
                    animator.SetBool(walkingHash, value);
                }
            }
        }

        float _walkingSpeed;
        public float walkingSpeed
        {
            get => _walkingSpeed;
            set
            {
                if (value != _walkingSpeed)
                {
                    _walkingSpeed = value;
                    animator.SetFloat(walkingSpeedHash, value);
                }
            }
        }

        bool _running;
        public bool running
        {
            get => _running;
            set
            {
                if (value != _running)
                {
                    _running = value;
                    animator.SetBool(runningHash, value);
                }
            }
        }

        public bool jump
        {
            set
            {
                if (value)
                {
                    animator.SetTrigger(jumpHash);
                }
                else
                {
                    animator.ResetTrigger(jumpHash);
                }
            }
        }

        bool _grounded;
        public bool grounded
        {
            get => _grounded;
            set
            {
                if (value != _grounded)
                {
                    _grounded = value;
                    animator.SetBool(groundedHash, value);
                }
            }
        }

        float _fallingSpeed;
        public float fallingSpeed
        {
            get => _fallingSpeed;
            set
            {
                if (value != _fallingSpeed)
                {
                    _fallingSpeed = value;
                    animator.SetFloat(fallingSpeedHash, value);
                }
            }
        }

        bool _lockedOn;
        public bool lockedOn
        {
            get => _lockedOn;
            set
            {
                if (value != _lockedOn)
                {
                    _lockedOn = value;
                    animator.SetBool(lockedOnHash, value);
                }
            }
        }

        float _walkingDirection;
        public float walkingDirection
        {
            get => _walkingDirection;
            set
            {
                if (value != _walkingDirection)
                {
                    _walkingDirection = value;
                    animator.SetFloat(walkingDirectionHash, value);
                }
            }
        }
    }

    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected AminationData animationData;

    protected CharacterController controller;

    protected virtual void Awake()
    {
        animationData.walkingHash = Animator.StringToHash("walking");
        animationData.walkingSpeedHash = Animator.StringToHash("speed");
        animationData.runningHash = Animator.StringToHash("running");
        animationData.jumpHash = Animator.StringToHash("jump");
        animationData.groundedHash = Animator.StringToHash("grounded");
        animationData.fallingSpeedHash = Animator.StringToHash("fallingSpeed");
        animationData.lockedOnHash = Animator.StringToHash("lockedOn");
        animationData.walkingDirectionHash = Animator.StringToHash("walkingDirection");
    }

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    protected void JumpAndGravity(bool jump)
    {
        animationData.jump = jump;
        if (characterData.Grounded)
        {
            characterData._fallTimeoutDelta = characterData.FallTimeout;

            if (characterData._verticalVelocity < 0.0f)
            {
                characterData._verticalVelocity = -2f;
            }

            if (jump && characterData.isAlive && characterData._jumpTimeoutDelta <= 0.0f)
            {
                characterData._verticalVelocity = Mathf.Sqrt(characterData.JumpHeight * -2f * characterData.Gravity);
            }

            if (characterData._jumpTimeoutDelta >= 0.0f)
            {
                characterData._jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            characterData._jumpTimeoutDelta = characterData.JumpTimeout;
            if (characterData._fallTimeoutDelta >= 0.0f)
            {
                characterData._fallTimeoutDelta -= Time.deltaTime;
            }

            animationData.fallingSpeed = characterData._verticalVelocity;
        }

        if (characterData._verticalVelocity < characterData._terminalVelocity)
        {
            characterData._verticalVelocity += characterData.Gravity * Time.deltaTime;
        }
    }

    protected void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - characterData.GroundedOffset,
            transform.position.z);
        characterData.Grounded = Physics.CheckSphere(spherePosition, characterData.GroundedRadius, characterData.GroundLayers,
            QueryTriggerInteraction.Ignore);

        animationData.grounded = characterData.Grounded;

        if (characterData.Grounded && characterData._verticalVelocity < -9.4)
        {
            characterData.isAlive = false;
        }
    }

    protected virtual void Move(Vector2 move, bool sprinting, float offsetCameraAngle = 0)
    {
        if (!characterData.isAlive)
        {
            move = Vector2.zero;
            sprinting = false;
        }


        float targetSpeed = sprinting ? characterData.SprintSpeed : characterData.MoveSpeed;

        if (move == Vector2.zero)
        {
            targetSpeed = 0.0f;
            animationData.walking = false;
        }
        else
        {
            animationData.walking = true;
            animationData.running = sprinting;
        }

        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            characterData._speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * characterData.SpeedChangeRate);

            characterData._speed = Mathf.Round(characterData._speed * 1000f) / 1000f;
        }
        else
        {
            characterData._speed = targetSpeed;
        }

        animationData.walkingSpeed = characterData._speed;

        if (move != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

            characterData._targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                offsetCameraAngle;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, characterData._targetRotation, ref characterData._rotationVelocity,
                characterData.RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, characterData._targetRotation, 0.0f) * Vector3.forward;

        controller.Move(targetDirection.normalized * (characterData._speed * Time.deltaTime) +
           new Vector3(0.0f, characterData._verticalVelocity * Time.deltaTime, 0.0f));
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (characterData.Grounded)
        {
            Gizmos.color = transparentGreen;
        }
        else
        {
            Gizmos.color = transparentRed;
        }

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - characterData.GroundedOffset, transform.position.z),
             characterData.GroundedRadius);
    }
}
