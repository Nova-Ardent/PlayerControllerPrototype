using System.Collections;
using System.Collections.Generic;
using Utilities;
using UnityEngine;
using Objects.Interactable;
using static Utilities.Controller.Controller;

namespace Creatures.Player
{
    public class PlayerCameraController : ILockable
    {
        float xRotation;
        float yRotation;

        PlayerCameraData playerCameraData;
        GameObject cameraArm;
        Camera camera;
        Transform target;

        InteractableObject lockedBy;

        public bool IsLocked
        {
            get => lockedBy != null;
        }

        public Vector2 Direction
        {
            get => xRotation.DegreeToVector2();
        }

        public float Angle
        {
            get => xRotation;
        }

        public PlayerCameraController(
            PlayerCameraData playerCameraData,
            GameObject cameraArm,
            Camera camera,
            Transform target)
        {
            this.playerCameraData = playerCameraData;
            this.cameraArm = cameraArm;
            this.camera = camera;
            this.target = target;

            Initialize();
        }

        void Initialize()
        {
            this.camera.transform.localPosition = this.playerCameraData.CameraArmLength;
            this.camera.transform.localRotation = Quaternion.Euler(this.playerCameraData.CameraAngle);

            Cursor.lockState = this.playerCameraData.CursorLocked;
        }

        public void Update()
        {
#if UNITY_EDITOR
            Initialize();
#endif

            if (!IsLocked)
            {
                UpdateCameraPosition();
                UpdateCameraRotation();
            }
        }

        public void UpdateCameraPosition()
        {
            this.cameraArm.transform.position = Vector3.LerpUnclamped(
                this.target.transform.position,
                this.cameraArm.transform.position,
                this.playerCameraData.PositionSnapSpeed);
        }

        public void UpdateCameraRotation()
        {
            this.xRotation += GetAxis(Controls.CameraMovementX) * this.playerCameraData.RotationSpeedX * Time.deltaTime;
            if (this.xRotation < 0)
            {
                this.xRotation += 360f;
            }
            else if (this.xRotation > 360f)
            {
                this.xRotation -= 360f;
            }

            this.yRotation += GetAxis(Controls.CameraMovementY) * this.playerCameraData.RotationSpeedY * Time.deltaTime;
            this.yRotation = Mathf.Clamp(
                this.yRotation,
                this.playerCameraData.MinYRotation,
                this.playerCameraData.MaxYRotation);


            Quaternion rotation = Quaternion.Euler(-this.yRotation, this.xRotation, 0f);
            this.cameraArm.transform.localRotation = Quaternion.LerpUnclamped(
                this.cameraArm.transform.localRotation,
                rotation,
                this.playerCameraData.RotationSnapSpeed);
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
