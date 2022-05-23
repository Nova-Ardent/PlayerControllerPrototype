using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalCharacter : PersonEditable
{
    [System.Serializable]
    public struct CameraData
    {
        [SerializeField] public GameObject cameraBase;
        [SerializeField] public GameObject cameraArm;
        [SerializeField] public float rotationSpeed;
        [SerializeField] public float angleSpeed;
        [SerializeField] public float zoomSpeed;
        [SerializeField] public Vector2 zoomVector;


        public float cameraRotation;
        public float cameraZoom;
        public float cameraAngle;
        public float cameraArmAngle;
        public float minAngle;
        public float maxAngle;
    }

    [SerializeField] Callouts callouts;
    [SerializeField] CameraData cameraData;

    void Start()
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

        SetCameraZoom();
        RegisterDebug();
    }

    private void OnDestroy()
    {
        UnregisterDebug();
    }


    void Update()
    {
        UpdateMovement();
        UpdateCamera();
    }

    void UpdateCamera()
    {
        bool rotationUpdated = false;

        var newHorizontal = Controller.GetAxis(Controller.Controls.CameraHorizontal);
        var newVertical = Controller.GetAxis(Controller.Controls.CameraVertical);

        if (newHorizontal != 0)
        {
            cameraData.cameraRotation += newHorizontal * cameraData.rotationSpeed;
            if (cameraData.cameraRotation > 315)
            {
                cameraData.cameraRotation -= 360;
            }
            else if (cameraData.cameraRotation < -45)
            {
                cameraData.cameraRotation += 360;
            }

            rotationUpdated = true;
        }
        
        if (newVertical != 0)
        {
            cameraData.cameraArmAngle -= newVertical * cameraData.rotationSpeed;
            if (cameraData.cameraArmAngle < cameraData.minAngle)
            {
               cameraData.cameraArmAngle = cameraData.minAngle;
            }
            else if (cameraData.cameraArmAngle > cameraData.maxAngle)
            {
                cameraData.cameraArmAngle = cameraData.maxAngle;
            }

            rotationUpdated = true;
        }
        
        if (rotationUpdated)
        {
            cameraData.cameraArm.transform.localRotation = Quaternion.Euler(cameraData.cameraArmAngle, cameraData.cameraRotation, 0);
        }
    }

    void SetCameraZoom()
    {
        cameraData.cameraBase.transform.localPosition = new Vector3(0, cameraData.zoomVector.y, cameraData.zoomVector.x);
    }

    void UpdateMovement()
    {
        var movement = new Vector2
        ( Controller.GetAxis(Controller.Controls.CharacterMovementHorizontal)
        , Controller.GetAxis(Controller.Controls.CharacterMovementVertical)
        );

        if (movement != Vector2.zero)
        {
            MoveDirection(Vector2.SignedAngle(movement, Vector2.down) + cameraData.cameraRotation);
        }
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
