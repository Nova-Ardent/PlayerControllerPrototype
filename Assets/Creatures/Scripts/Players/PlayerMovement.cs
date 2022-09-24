using System.Collections;
using System.Collections.Generic;
using Utilities;
using UnityEngine;
using Objects.Interactable;
using static Utilities.Controller.Controller;

namespace Creatures.Player
{
    public class PlayerMovement : MovementBase, ILockable
    {
        PlayerCameraController playerCameraController;
        PlayerMovementData playerMovementData;
        PlayerAnimator playerAnimator;
        InteractableObject lockedBy;

        public bool IsLocked
        {
            get => lockedBy != null;
        }

        public PlayerMovement(
            Transform transform,
            PlayerMovementData playerMovementData,
            PlayerCameraController playerCameraController,
            PlayerAnimator playerAnimator,
            RagdollController ragdollController) 
            : base(transform, playerMovementData, playerAnimator, ragdollController)
        {
            if (!Utilities.Controller.Controller.ControllerInitialized)
            {
                Debug.LogWarning("Controller not initialized, no input functionality will be available.");
            }

            this.playerCameraController = playerCameraController;
            this.playerMovementData = playerMovementData;
            this.playerAnimator = playerAnimator;
        }

        public override void Update()
        {
            if (isDead)
            {
                return;
            }

            if (!IsLocked)
            {
                UpdatePosition();
                base.Update();
            }
        }

        public override void UpdateJump()
        {
            if (GetKeyDown(Controls.Jump) && CanJump)
            {
                jumpQueued = true;
                timeTillJump = movementData.JumpDelay;
            }

            base.UpdateJump();
        }

        public void UpdatePosition()
        {
            if (isGrounded && !RecentlyJumped && !jumpQueued)
            {
                running = GetKey(Controls.Run);

                float x = -GetAxis(Controls.MovementX);
                float y = GetAxis(Controls.MovementY);
                if (x != 0 || y != 0)
                {
                    direction = Vector2.SignedAngle(Vector2.up, new Vector2(x, y)) + playerCameraController.Angle;

                    Vector2 movement = direction.DegreeToVector2();
                    xMovement = movement.x;
                    yMovement = movement.y;
                }
                else
                {
                    xMovement = 0;
                    yMovement = 0;
                }
            }
        }

        public void Unlock()
        {
            this.lockedBy = null;
        }

        public void Lock(InteractableObject lockBy)
        {
            this.lockedBy = lockBy;   
        }
    }
}