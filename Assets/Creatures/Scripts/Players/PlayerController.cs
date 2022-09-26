using System.Collections;
using System.Collections.Generic;
using Objects.UI;
using UnityEngine;
using System;
using Creatures.Equippables;
using Creatures.Player.Camera;

namespace Creatures.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        public class PlayerAnimationInfo
        {
            public Animator animator;
        }

        [Serializable]
        public class PlayerMovementInfo
        {
            public GameObject cameraArm;
            public UnityEngine.Camera camera;
            public PlayerCameraData cameraData;
            public Transform cameraFollowTarget;

            public Collider collider;
            public PlayerMovementData movementData;
        }

        [Serializable]
        public class CalloutControllerInfo
        {
            public GameObject calloutBase;
            public ButtonCallout[] callouts;
        }

        [Serializable]
        public class RagdollInfos
        {
            public Rigidbody[] ragdollRigidbodies;
            public Collider[] ragdollColliders;
        }

        PlayerAnimator playerAnimator;
        PlayerMovement playerMovement;
        PlayerCameraController playerCameraController;
        CalloutController calloutController;
        RagdollController ragdollController;

        [SerializeField] PlayerMovementInfo playerMovementInfo;
        [SerializeField] PlayerAnimationInfo playerAnimationInfo;
        [SerializeField] CalloutControllerInfo calloutControllerInfo;
        [SerializeField] RagdollInfos ragdollInfo;
        [SerializeField] PlayerEquippable playerEquippable;

        public PlayerMovement PlayerMovement
        {
            get => playerMovement;
        }

        public PlayerCameraController PlayerCameraController
        {
            get => playerCameraController;
        }

        public CalloutController CalloutController
        {
            get => calloutController;
        }

        public RagdollInfos RagdollInfo
        {
            get => ragdollInfo;
        }

        public PlayerEquippable PlayerEquippable
        {
            get => playerEquippable;
        }

        // Start is called before the first frame update
        void Start()
        {
            playerAnimator = new PlayerAnimator(
                playerAnimationInfo.animator);

            playerCameraController = new PlayerCameraController(
                playerMovementInfo.cameraData,
                playerMovementInfo.cameraArm,
                playerMovementInfo.camera,
                playerMovementInfo.cameraFollowTarget);

            ragdollController = new RagdollController(
                ragdollInfo.ragdollRigidbodies,
                ragdollInfo.ragdollColliders,
                playerAnimationInfo.animator,
                playerMovementInfo.collider
                );

            playerMovement = new PlayerMovement(
                this.transform,
                this.playerMovementInfo.movementData,
                this.playerCameraController,
                this.playerAnimator,
                this.ragdollController);

            calloutController = new CalloutController(
                calloutControllerInfo.calloutBase,
                calloutControllerInfo.callouts,
                transform,
                playerMovementInfo.camera.transform
                );
        }

        // Update is called once per frame
        void Update()
        {
            playerMovement.Update();
            playerCameraController.Update();
            calloutController.Update();
        }

        private void OnDrawGizmosSelected()
        {
            playerMovement?.DrawGrounded();
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}