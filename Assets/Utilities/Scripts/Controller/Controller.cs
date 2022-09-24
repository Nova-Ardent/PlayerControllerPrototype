using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Controller
{
    public static class Controller
    {
        public enum Controls
        {
            Jump,

            CameraMovementX,
            CameraMovementY,

            MovementX,
            MovementY,
            Run,

            InteractOne,
            InteractTwo,
            InteractThree,
            InteractFour,
        }

        public enum InputAlias
        {
        }

        public enum ControllerType
        {
            keyboard,
            ps,
            xbox,
            automation,
        }

        static bool controllersInitialized;
        static ControllerBase controllerSetup;
        static Keyboard keyboard = new Keyboard();
        static XboxController xboxController = new XboxController();
        static PSController pSController = new PSController();

        static ControllerBase currentController;
        static ControllerType _currentControllerType;
        
        public static bool ControllerInitialized
        {
            get => controllersInitialized;
        }

        public static ControllerType currentControllerType
        {
            get => _currentControllerType;
            set
            {
                _currentControllerType = value;
                switch (value)
                {
                    case ControllerType.keyboard: currentController = keyboard; break;
                    case ControllerType.ps: currentController = pSController; break;
                    case ControllerType.xbox: currentController = xboxController; break;
                }
            }
        }

        public static ControllerType GetControllerType()
        {
            string[] joystickNames = Input.GetJoystickNames();
            foreach (var joystickName in joystickNames)
            {
                if (joystickName.ToLower().Contains("xbox"))
                {
                    return ControllerType.xbox;
                }
                else if (joystickName.ToLower().Contains("playstation"))
                {
                    return ControllerType.ps;
                }
                else
                {
                    return ControllerType.keyboard;
                }
            }
            return ControllerType.keyboard;
        }

        public static void SetControllerType(ControllerType? controllerType = null)
        {
            if (controllerType == null)
            {
                currentControllerType = GetControllerType();
                return;
            }
            currentControllerType = (ControllerType)controllerType;
        }

        static void SetupButtonAxis(InputAlias alias, Controls control)
        {
            controllerSetup?.SetupButtonAxis(alias, control);
        }

        static void SetupButtonAxis(string alias, Controls control)
        {
            controllerSetup?.SetupButtonAxis(alias, control);
        }

        static void SetupButtonAxis(KeyCode key, KeyCode key2, Controls control)
        {
            controllerSetup?.SetupButtonAxis(key, key2, control);
        }

        static void SetupButtonAxis(KeyCode key, int pressValue, Controls control)
        {
            controllerSetup?.SetupButtonAxis(key, pressValue, control);
        }

        static void SetupButtonHeld(KeyCode key, Controls control)
        {
            controllerSetup?.SetupButtonHeld(key, control);
        }

        static void SetupButtonDown(KeyCode key, Controls control)
        {
            controllerSetup?.SetupButtonDown(key, control);
        }

        static void SetupButtonUp(KeyCode key, Controls control)
        {
            controllerSetup?.SetupButtonUp(key, control);
        }

        static void SetupButtonHeld(InputAlias key, Controls control)
        {
            controllerSetup?.SetupButtonHeld(key, control);
        }

        static void SetupButtonDown(InputAlias key, Controls control)
        {
            controllerSetup?.SetupButtonDown(key, control);
        }

        static void SetupButtonUp(InputAlias key, Controls control)
        {
            controllerSetup?.SetupButtonUp(key, control);
        }

        static void SetupButtonHeld(string key, Controls control)
        {
            controllerSetup?.SetupButtonHeld(key, control);
        }

        static void SetupButtonDown(string key, Controls control)
        {
            controllerSetup?.SetupButtonDown(key, control);
        }

        static void SetupButtonUp(string key, Controls control)
        {
            controllerSetup?.SetupButtonUp(key, control);
        }

        static void SetupMouseButtonHeld(int button, Controls control)
        {
            controllerSetup?.SetupMouseButtonHeld(button, control);
        }

        static void SetupMouseButtonDown(int button, Controls control)
        {
            controllerSetup?.SetupMouseButtonDown(button, control);
        }

        static void SetupMouseButtonUp(int button, Controls control)
        {
            controllerSetup?.SetupMouseButtonUp(button, control);
        }


        public static bool GetKey(Controls control)
        {
            return currentController?.GetKey(control) ?? false;
        }

        public static bool GetKeyUp(Controls control)
        {
            return currentController?.GetKeyUp(control) ?? false;
        }

        public static bool GetKeyDown(Controls control)
        {
            return currentController?.GetKeyDown(control) ?? false;
        }

        public static float GetAxis(Controls control)
        {
            return currentController?.GetAxis(control) ?? 0;
        }

        public static void SetupControllers()
        {
            controllerSetup = keyboard;
            SetupKeyboard();

            controllerSetup = xboxController;
            SetupXbox();

            controllerSetup = pSController;
            SetupPS();

            controllersInitialized = true;
            controllerSetup = null;
        }

        static void SetupKeyboard()
        {
            SetupButtonDown(KeyCode.Space, Controls.Jump);

            SetupButtonAxis("Mouse X", Controls.CameraMovementX);
            SetupButtonAxis("Mouse Y", Controls.CameraMovementY);

            SetupButtonAxis(KeyCode.W, KeyCode.S, Controls.MovementY);
            SetupButtonAxis(KeyCode.D, KeyCode.A, Controls.MovementX);

            SetupButtonHeld(KeyCode.LeftShift, Controls.Run);

            SetupButtonDown(KeyCode.Alpha1, Controls.InteractOne);
            SetupButtonDown(KeyCode.Alpha2, Controls.InteractTwo);
            SetupButtonDown(KeyCode.Alpha3, Controls.InteractThree);
            SetupButtonDown(KeyCode.Alpha4, Controls.InteractFour);
        }

        static void SetupXbox()
        {
        }

        static void SetupPS()
        {
        }
    }
}