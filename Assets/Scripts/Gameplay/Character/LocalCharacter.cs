using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalCharacter : PersonEditable
{
    [System.Serializable]
    public class CameraData
    {
        public Camera mainCamera;
        public GameObject cameraArm;
        public GameObject cameraArmTip;

        public float currentRotation = 0;
        public float currentTilt = 0;
        public float tiltMin = -20;
        public float tiltMax = 40;

        public float lerp = 1.0f;
    }


    [SerializeField] Callouts callouts;
    [SerializeField] CameraData cameraData;

    protected override void Start()
    {
        Localized.Instance.ValidateAndCreateLanguages();
        Localized.Instance.SetLanguage(0);

        if (!Controller.controllersAreSetup)
        {
            Controller.SetupControllers(callouts);
            Controller.SetControllerType(Controller.ControllerType.keyboard);
        }

        if (SteamManager.Initialized)
        {
            characterData.name = Steamworks.SteamFriends.GetPersonaName();
        }

        RegisterDebug();
        base.Start();
    }

    void OnDestroy()
    {
        UnregisterDebug();
    }


    void Update()
    {
        JumpAndGravity(Controller.GetKey(Controller.Controls.CharacterJump));
        GroundedCheck();

        Move
        ( new Vector2
            (Controller.GetAxis(Controller.Controls.CharacterMovementHorizontal)
            , Controller.GetAxis(Controller.Controls.CharacterMovementVertical)
            )
        , Controller.GetKey(Controller.Controls.CharacterSprint)
        , cameraData.mainCamera.transform.rotation.eulerAngles.y + 180);

        UpdateCamera();
    }

    void UpdateCamera()
    {
        cameraData.cameraArm.transform.position = this.transform.position;

        float rotation = Controller.GetAxis(Controller.Controls.CameraHorizontal);
        float tilt = Controller.GetAxis(Controller.Controls.CameraVertical);
        if (rotation != 0 || tilt != 0)
        {
            cameraData.currentRotation += rotation;
            if (cameraData.currentRotation > 360)
            {
                cameraData.currentRotation -= 360;
            }
            else if (cameraData.currentRotation < 0)
            {
                cameraData.currentRotation += 360;
            }

            cameraData.currentTilt -= tilt;
            if (cameraData.currentTilt < cameraData.tiltMin)
            {
                cameraData.currentTilt = cameraData.tiltMin;
            }
            else if (cameraData.currentTilt > cameraData.tiltMax)
            {
                cameraData.currentTilt = cameraData.tiltMax;
            }

            cameraData.cameraArm.transform.localRotation = Quaternion.Euler(cameraData.currentTilt, cameraData.currentRotation, 0);
        }

        cameraData.mainCamera.transform.position = Vector3.Lerp(cameraData.mainCamera.transform.position, cameraData.cameraArmTip.transform.position, cameraData.lerp * Time.deltaTime);
        cameraData.mainCamera.transform.rotation = Quaternion.Lerp(cameraData.mainCamera.transform.rotation, cameraData.cameraArmTip.transform.rotation, cameraData.lerp * Time.deltaTime);
    }

    // to do anim
    private void AssignAnimationIDs()
    {
        /*_animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");*/
    }

    void RegisterDebug()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugMenu.DebugMenu.Instance.RegisterPanel
        ("Player", this
        );
#endif
    }

    void UnregisterDebug()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugMenu.DebugMenu.Instance.UnRegisterPanel("Player");
#endif
    }
}
