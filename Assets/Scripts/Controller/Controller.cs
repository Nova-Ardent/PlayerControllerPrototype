#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Callouts;

public static class Controller
{
    public static Callouts? callouts { get; private set; }

    public enum Controls
    {
        MouseVertical,
        MouseHorizontal,

        MenuNavUp,
        MenuNavLeft,
        MenuNavRight,
        MenuNavDown,

        CameraVertical,
        CameraHorizontal,

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // Debug only controls. Do not use them outside of the debug menu.
        DebugMenuOpen,
        DebugMenuClose,

        DebugUp,
        DebugRight,
        DebugDown,
        DebugLeft,
        DebugPageUp,
        DebugPageDown,
    }

    public enum KeyType
    {
        Regular,
        Debug,
        Both,
#endif
    }

    public enum InputAlias
    {
        [InputAlias("Mouse Y")] MouseVertical,
        [InputAlias("Mouse X")] MouseHorizontal,

        [InputAlias("Vertical")] xboxVertical,
        [InputAlias("Horizontal")] xboxHorizontal,

        [InputAlias("Vertical")] pSVertical,
        [InputAlias("Horizontal")] pSHorizontal,

        [InputAlias("joystick button 0")] xboxA,
        [InputAlias("joystick button 0")] pSX,

        [InputAlias("joystick button 16")] xboxAOSX,
        [InputAlias("joystick button 16")] psXOSX,
    }

    public enum ControllerType
    {
        keyboard,
        ps,
        xbox,
        automation,
    }

    public static bool controllersAreSetup { get; private set; }
    static ControllerBase? controllerSetup;
    static Keyboard keyboard = new Keyboard();
    static XboxController xboxController = new XboxController();
    static PSController pSController = new PSController();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public static bool debugMenuOpen = false;
    static KeyType[] debugMenuControls = new KeyType[Utilities.GetEnums<Controls>().Count()];
#endif

    static ControllerBase? currentController;
    static ControllerType _currentControllerType;
    public static ControllerType currentControllerType 
    {
        get => _currentControllerType;
        set {
            _currentControllerType = value;
            switch (value)
            {
                case ControllerType.keyboard:       currentController = keyboard;               break;
                case ControllerType.ps:             currentController = pSController;           break;
                case ControllerType.xbox:           currentController = xboxController;         break;
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

#region Setup
    static void SetupButtonAxis(InputAlias alias, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonAxis(alias, control, callout);
    }

    static void SetupButtonAxis(string alias, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonAxis(alias, control, callout);
    }

    static void SetupButtonAxis(KeyCode key, KeyCode key2, Controls control, Sprite? callout, Sprite? callout2)
    {
        controllerSetup?.SetupButtonAxis(key, key2, control, callout, callout2);
    }

    static void SetupButtonAxis(KeyCode key, int pressValue, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonAxis(key, pressValue, control, callout);
    }

    static void SetupButtonHeld(KeyCode key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonHeld(key, control, callout);
    }

    static void SetupButtonDown(KeyCode key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonDown(key, control, callout);
    }

    static void SetupButtonUp(KeyCode key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonUp(key, control, callout);
    }

    static void SetupButtonHeld(InputAlias key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonHeld(key, control, callout);
    }

    static void SetupButtonDown(InputAlias key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonDown(key, control, callout);
    }

    static void SetupButtonUp(InputAlias key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonUp(key, control, callout);
    }


    static void SetupButtonHeld(string key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonHeld(key, control, callout);
    }

    static void SetupButtonDown(string key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonDown(key, control, callout);
    }

    static void SetupButtonUp(string key, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupButtonUp(key, control, callout);
    }

    static void SetupMouseButtonHeld(int button, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupMouseButtonHeld(button, control, callout);
    }

    static void SetupMouseButtonDown(int button, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupMouseButtonDown(button, control, callout);
    }

    static void SetupMouseButtonUp(int button, Controls control, Sprite? callout)
    {
        controllerSetup?.SetupMouseButtonUp(button, control, callout);
    }
    #endregion
#region Get
    public static bool GetKey(Controls control)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (DebugWillEatInput(control))
        {
            return false;
        }
#endif 
        return currentController?.GetKey(control) ?? false;
    }

    public static bool GetKeyUp(Controls control)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (DebugWillEatInput(control))
        {
            return false;
        }
#endif 
        return currentController?.GetKeyUp(control) ?? false;
    }

    public static bool GetKeyDown(Controls control)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (DebugWillEatInput(control))
        {
            return false;
        }
#endif 
        return currentController?.GetKeyDown(control) ?? false;
    }

    public static float GetAxis(Controls control)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (DebugWillEatInput(control))
        {
            return 0;
        }
#endif 
        return currentController?.GetAxis(control) ?? 0;
    }

    public static Sprite? GetCallout(KeyCode control)
    {
        return callouts?.keyboardKeys[control];
    }

    public static Sprite? GetCallout(CalloutAliasXbox control)
    {
        return callouts?.xboxKeys[control];
    }

    public static Sprite? GetCallout(CalloutAliasPS control)
    {
        return callouts?.pSKeys[control];
    }

    public static Sprite[] GetCallouts(Controls control)
    {
        if (currentController != null)
        {
            return currentController.GetCallouts(control)
                .Select(x => x ?? Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f)))
                .ToArray();
        }
        return new Sprite[] { Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f)) };
    }
#endregion
    public static void SetupControllers(Callouts calloutsData)
    {
        callouts = calloutsData;

        controllerSetup = keyboard;
        SetupKeyboard();

        controllerSetup = xboxController;
        SetupXbox();

        controllerSetup = pSController;
        SetupPS();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        DebugKeys();
#endif

        controllersAreSetup = true;
        controllerSetup = null;
    }

    static void SetupKeyboard()
    {
        SetupButtonDown(KeyCode.UpArrow, Controls.MenuNavUp, callouts?.keyboardKeys[KeyCode.UpArrow]);
        SetupButtonDown(KeyCode.LeftArrow, Controls.MenuNavLeft, callouts?.keyboardKeys[KeyCode.LeftArrow]);
        SetupButtonDown(KeyCode.RightArrow, Controls.MenuNavRight, callouts?.keyboardKeys[KeyCode.RightArrow]);
        SetupButtonDown(KeyCode.DownArrow, Controls.MenuNavDown, callouts?.keyboardKeys[KeyCode.DownArrow]);

        SetupButtonUp(KeyCode.UpArrow, Controls.MenuNavUp, callouts?.keyboardKeys[KeyCode.UpArrow]);
        SetupButtonUp(KeyCode.LeftArrow, Controls.MenuNavLeft, callouts?.keyboardKeys[KeyCode.LeftArrow]);
        SetupButtonUp(KeyCode.RightArrow, Controls.MenuNavRight, callouts?.keyboardKeys[KeyCode.RightArrow]);
        SetupButtonUp(KeyCode.DownArrow, Controls.MenuNavDown, callouts?.keyboardKeys[KeyCode.DownArrow]);

        SetupButtonAxis(InputAlias.MouseVertical, Controls.MouseVertical, callouts?.mouse);
        SetupButtonAxis(InputAlias.MouseHorizontal, Controls.MouseHorizontal, callouts?.mouse);

        SetupButtonAxis(InputAlias.MouseVertical, Controls.CameraVertical, callouts?.mouse);
        SetupButtonAxis(InputAlias.MouseHorizontal, Controls.CameraHorizontal, callouts?.mouse);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        SetupButtonDown(KeyCode.BackQuote, Controls.DebugMenuOpen, null);
        SetupButtonDown(KeyCode.Escape, Controls.DebugMenuClose, null);
        SetupButtonDown(KeyCode.UpArrow, Controls.DebugUp, null);
        SetupButtonDown(KeyCode.RightArrow, Controls.DebugRight, null);
        SetupButtonDown(KeyCode.DownArrow, Controls.DebugDown, null);
        SetupButtonDown(KeyCode.LeftArrow, Controls.DebugLeft, null);
        SetupButtonDown(KeyCode.PageUp, Controls.DebugPageUp, null);
        SetupButtonDown(KeyCode.PageDown, Controls.DebugPageDown, null);
#endif
    }

    static void SetupXbox()
    {
    }

    static void SetupPS()
    {
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    static void RegisterDebugAsButton(Controls control, KeyType keyType = KeyType.Debug)
    {
        debugMenuControls[(int)control] = keyType;
    }

    static void DebugKeys()
    {
        RegisterDebugAsButton(Controls.DebugMenuOpen, KeyType.Both);
        RegisterDebugAsButton(Controls.DebugMenuClose, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugUp, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugRight, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugDown, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugLeft, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugPageUp, KeyType.Debug);
        RegisterDebugAsButton(Controls.DebugPageDown, KeyType.Debug);
    }

    static bool DebugWillEatInput(Controls control)
    {
        if (debugMenuControls[(int)control] == KeyType.Both)
        {
            return false;
        }

        if (debugMenuControls[(int)control] == KeyType.Regular && debugMenuOpen)
        {
            return true;
        }

        if (debugMenuControls[(int)control] == KeyType.Debug && !debugMenuOpen)
        {
            return true;
        }

        return false;
    }
#endif
}
