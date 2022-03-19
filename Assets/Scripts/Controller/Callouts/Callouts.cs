using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Callouts", menuName = "ScriptableObjects/Callouts", order = 1)]
public class Callouts : ScriptableObject
{
    public enum CalloutAliasXbox
    {
        A,
        B,
        X,
        Y,
        DPad,
        DPadDown,
        DPadLeft,
        DPadLeftRight,
        DPadRight,
        DPadUp,
        DPadUpDown,
        LB,
        LT,
        RB, 
        RT,
        Menu,
        View,
        LeftStick,
        LeftStickPress,
        RightStick,
        RightStickPress,
    }

    public enum CalloutAliasPS
    {
        X,
        Circle,
        Square,
        Triangle,
        DPad,
        DPadDown,
        DPadLeft,
        DPadLeftRight,
        DPadRight,
        DPadUp,
        DPadUpDown,
        L1,
        L2,
        R1,
        R2,
        Options,
        Pad,
        Share,
        LeftStick,
        LeftStickPress,
        RightStick,
        RightStickPress,
    }

    public DataMap<KeyCode, Sprite> keyboardKeys = new DataMap<KeyCode, Sprite>();
    public DataMap<CalloutAliasXbox, Sprite> xboxKeys = new DataMap<CalloutAliasXbox, Sprite>();
    public DataMap<CalloutAliasPS, Sprite> pSKeys = new DataMap<CalloutAliasPS, Sprite>();

    public Sprite mouse;
    public Sprite mouseLeftClick;
    public Sprite mouseMiddleClick;
    public Sprite mouseRightClick;
}
