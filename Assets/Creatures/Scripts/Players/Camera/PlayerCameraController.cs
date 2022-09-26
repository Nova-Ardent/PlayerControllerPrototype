using System.Collections;
using System.Collections.Generic;
using Utilities;
using UnityEngine;
using Creatures.Lockable;
using Objects.Interactable;
using Utilities.Math;
using static Utilities.Controller.Controller;

namespace Creatures.Player.Camera
{
    public class PlayerCameraController : ILockable
    {
        float xRotation;
        float yRotation;

        PlayerCameraData playerCameraData;
        GameObject cameraArm;
        UnityEngine.Camera camera;
        Transform target;

        InteractableObject lockedBy;

        Quaternion cameraAngleFrom;
        Quaternion cameraAngleTo;
        Vector3 cameraArmLengthFrom;
        Vector3 cameraArmLengthTo;
        Lerper lerper = new Lerper(0.5f);

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
            UnityEngine.Camera camera,
            Transform target)
        {
            this.playerCameraData = playerCameraData;
            this.cameraArm = cameraArm;
            this.camera = camera;
            this.target = target;

            Cursor.lockState = CursorLockMode.Locked;
            SetLerper(this.playerCameraData.CameraArmLength, this.playerCameraData.CameraAngle);
        }

        public void Update()
        {
            UpdateCameraArm();
            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        public void UpdateCameraArm()
        {
            if (!lerper.Update())
            {
                this.camera.transform.localPosition = Vector3.Lerp(cameraArmLengthFrom, cameraArmLengthTo, lerper.Value);
                this.camera.transform.localRotation = Quaternion.Lerp(cameraAngleFrom, cameraAngleTo, lerper.Value);
            }
        }

        public void UpdateCameraPosition()
        {
            if (IsLocked && lockedBy is ICanLockCameraPosition cameraLock)
            {
                this.cameraArm.transform.position = Vector3.LerpUnclamped(
                    cameraLock.CameraFocalPoint.position,
                    this.cameraArm.transform.position,
                    this.playerCameraData.PositionSnapSpeed);
            }
            else
            {
                this.cameraArm.transform.position = Vector3.LerpUnclamped(
                    this.target.transform.position,
                    this.cameraArm.transform.position,
                    this.playerCameraData.PositionSnapSpeed);
            }
        }

        public void UpdateCameraRotation()
        {
            if (IsLocked && lockedBy is ICanLockCameraPosition cameraLock)
            {
                this.cameraArm.transform.localRotation = Quaternion.LerpUnclamped(
                    this.cameraArm.transform.localRotation,
                    cameraLock.CameraFocalPoint.rotation,
                    cameraLock.FocusPointData.RotationSnapSpeed);

                this.xRotation = cameraLock.CameraFocalPoint.rotation.eulerAngles.y;
                this.yRotation = -cameraLock.CameraFocalPoint.rotation.eulerAngles.x;
            }
            else
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
        }

        void SetLerper(Vector3 length, Vector3 angle)
        {
            cameraArmLengthFrom = this.camera.transform.localPosition;
            cameraAngleFrom = this.camera.transform.localRotation;

            cameraArmLengthTo = length;
            cameraAngleTo = Quaternion.Euler(angle);
            lerper.Reset();
        }

        public void Unlock(InteractableObject unlockBy)
        {
            if (unlockBy == this.lockedBy)
            {
                this.lockedBy = null;
                SetLerper(this.playerCameraData.CameraArmLength, this.playerCameraData.CameraAngle);
            }
        }

        public void Lock(InteractableObject lockBy)
        {
            if (lockBy is ICanLockCameraPosition cameraLock)
            {
                this.lockedBy = lockBy;
                SetLerper(
                    cameraLock.FocusPointData.CameraArmLength,
                    cameraLock.FocusPointData.RelativeAngleToFocus);
            }
        }
    }
}
