using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalCharacter : CharacterBase
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

        RegisterDebug();
    }

    private void OnDestroy()
    {
        UnregisterDebug();
    }


    void Update()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        var newRotation = Controller.GetAxis(Controller.Controls.CameraHorizontal);
        if (newRotation != 0)
        {
            cameraData.cameraRotation += newRotation * cameraData.rotationSpeed;
            if (cameraData.cameraRotation > 315)
            {
                cameraData.cameraRotation -= 360;
            }
            else if (cameraData.cameraRotation < -45)
            {
                cameraData.cameraRotation += 360;
            }

            cameraData.cameraBase.transform.localRotation = Quaternion.Euler(0, cameraData.cameraRotation, 0);
        }
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    void RegisterDebug()
    {
        DebugMenu.DebugMenu.Instance.RegisterPanel
        ( "Player"
        , new DebugMenu.DebugOption("name: ", characterData.name)
        );
    }

    void UnregisterDebug()
    {
        DebugMenu.DebugMenu.Instance.UnRegisterPanel("Player");
    }
#endif
}
