using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DebugMenu;
using System.Linq;

public class DebugMenuExample : MonoBehaviour
{
    enum Test
    {
        Option1,
        Option2,
        Option3,
        Option4,
    }

    // Start is called before the first frame update
    void Start()
    {

        0.Range(4).OrderBy(x => x).Select(x => { Debug.LogError(x); return x; }).ToArray();

        string lastPressed = "press me";

        DebugMenu.DebugMenu.Instance.RegisterPanel
            ( "debug menu example script"
            , new DebugOption("static name", "static description")
            , new DebugOption("static name", () => $"dynamic text: {DateTime.Now}")
            , new DebugOptionAction("Action", () => lastPressed, () => lastPressed = $"last pressed: {DateTime.Now}")
            , new DebugMenuSliderInt("integer test", 0, 20, 2, x => { Debug.Log($"incremented to: {x}"); } )
            , new DebugMenuSliderFloat("float test", 0, 20, 0.5f, x => { Debug.Log($"incremented to: {x}"); } )
            , new DebugMenuEnum<Test>("enum test", Test.Option3, (x) => { Debug.Log($"enum increment to: {x}"); } )
            );
    }
}
