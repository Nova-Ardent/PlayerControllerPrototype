using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class MovementBase
    {
        private Transform transform;
        private CharacterController collider;
        private CreatureAnimator creatureAnimator;
        private RagdollController ragdollController;

        // vertical movement
        protected Vector3 groundedSpherePos;
        protected bool groundedStateChange;
        protected bool isGrounded;
        protected float fallingSpeed;

        protected bool jumpQueued;
        protected float timeTillJump;

        protected float xMovement;
        protected float yMovement;
        protected float direction;
        protected Vector3 velocity;
        protected float walkingToRunning;
        protected bool running;

        protected bool isDead;

        protected MovementData movementData;

        public bool IsGrounded
        {
            get => isGrounded;
        }

        public bool RecentlyJumped
        {
            get => timeTillJump != 0;
        }

        public bool CanJump
        {
            get => IsGrounded && timeTillJump == 0;
        }

        public CharacterController Collider
        {
            get => collider;
        }

        public RagdollController RagdollController
        {
            get => ragdollController;
        }

        public bool IsDead
        {
            get => isDead;
            set
            {
                if (value)
                {
                    ragdollController.Activate(velocity);
                }
                isDead = value;
            }
        }

        public MovementBase(
            Transform transform,
            MovementData movementData,
            CreatureAnimator creatureAnimator, 
            RagdollController ragdollController)
        {
            this.transform = transform;
            this.movementData = movementData;
            this.creatureAnimator = creatureAnimator;
            this.ragdollController = ragdollController;

            if (!this.transform.TryGetComponent<CharacterController>(out collider))
            {
                Debug.LogError("No collider attached to the gameobject");
            }
        }

        public virtual void Update()
        {
            if (!isDead)
            {
                UpdateGrounded();
                UpdateMovement();
                UpdateJump();
                UpdateFalling();
                UpdateAnimator();
            }
        }

        public virtual void UpdateGrounded()
        {
            groundedSpherePos = new Vector3(
                transform.position.x + movementData.GroundedPosition.x,
                transform.position.y + movementData.GroundedPosition.y,
                transform.position.z + movementData.GroundedPosition.z);
            
            bool isGrounded = Physics.CheckSphere(
                groundedSpherePos,
                movementData.GroundedRadius,
                movementData.GroundLayers,
                QueryTriggerInteraction.Ignore);
            
            if (isGrounded != this.isGrounded)
            {
                groundedStateChange = true;
            }
            else
            {
                groundedStateChange = false;
            }
            this.isGrounded = isGrounded;
        }

        public virtual void UpdateJump()
        {
            if (jumpQueued)
            {
                var info = creatureAnimator.Animator.GetCurrentAnimatorStateInfo(0);
                if (xMovement != 0 || yMovement != 0)
                {
                    if (info.normalizedTime > movementData.MovingJumpAtNormalized)
                    {
                        jumpQueued = false;
                        fallingSpeed = movementData.JumpStrength;
                    }
                }
                else
                {
                    if (info.normalizedTime > movementData.JumpAtNormalized)
                    {
                        jumpQueued = false;
                        fallingSpeed = movementData.JumpStrength;
                    }
                }
            }

            if (timeTillJump > 0)
            {
                timeTillJump -= Time.deltaTime;
            }
            else 
            {
                timeTillJump = 0;
            }
        }

        public virtual void UpdateFalling()
        {
            if (isGrounded && !RecentlyJumped)
            {
                fallingSpeed = -2;
            }
            else
            {
                if (groundedStateChange && !RecentlyJumped)
                {
                    fallingSpeed = 0;
                }

                fallingSpeed += movementData.Gravity * Time.deltaTime;

                if (movementData.DeathFallingSpeed > fallingSpeed)
                {
                    IsDead = true;
                }
            }
        }

        public virtual void UpdateMovement()
        {
            Vector2 movement = Vector2.zero;
            if (xMovement != 0 || yMovement != 0)
            {
                if (running)
                {
                    walkingToRunning = Mathf.Min(walkingToRunning + Time.deltaTime / movementData.MovementTransitionTime, 1);
                }
                else
                {
                    walkingToRunning = Mathf.Max(walkingToRunning - Time.deltaTime / movementData.MovementTransitionTime, 0);
                }
                
                movement = new Vector2(xMovement, yMovement).normalized * Mathf.LerpUnclamped(movementData.WalkingSpeed, movementData.RunningSpeed, walkingToRunning);
            }
            else
            {
                walkingToRunning = 0;
            }

            transform.localRotation = Quaternion.LerpUnclamped(
                transform.localRotation,
                Quaternion.Euler(0, direction, 0),
                movementData.RotationSpeed);

            velocity = new Vector3(movement.y, fallingSpeed, movement.x);
            collider.Move(velocity * Time.deltaTime);
        }

        public virtual void UpdateAnimator()
        {
            if (jumpQueued || RecentlyJumped)
            {
                if (xMovement != 0 || yMovement != 0)
                {
                    creatureAnimator.SetStateMovingJump();
                }
                else
                {
                    creatureAnimator.SetStateJump();
                }
                return;
            }

            if (!isGrounded)
            {
                creatureAnimator.SetStateFalling();
                return;
            }

            if (xMovement != 0 || yMovement != 0)
            {
                creatureAnimator.SetSpeed(walkingToRunning);
                creatureAnimator.SetStateWalking();
                return;
            }

            creatureAnimator.SetStateIdle();
        }

        public void DrawGrounded()
        {
            if (isGrounded)
            {
                Gizmos.color = movementData.GroundedDrawColor;
            }
            else
            {
                Gizmos.color = movementData.NotGroundedDrawColor;
            }

            Gizmos.DrawWireSphere(groundedSpherePos, movementData.GroundedRadius);
        }
    }
}
