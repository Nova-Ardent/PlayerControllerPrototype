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

    static bool controllersAreSetup;
    static ControllerBase? controllerSetup;
    static Keyboard keyboard = new Keyboard();
    static XboxController xboxController = new XboxController();
    static PSController pSController = new PSController();

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

    public static void SetupControllers(Callouts calloutsData)
    {
        callouts = calloutsData;

        controllerSetup = keyboard;
        SetupKeyboard();

        controllerSetup = xboxController;
        SetupXbox();

        controllerSetup = pSController;
        SetupPS();

        controllersAreSetup = true;
        controllerSetup = null;
    }

    static void SetupKeyboard()
    {
    }

    static void SetupXbox()
    {
    }

    static void SetupPS()
    {
    }
}
