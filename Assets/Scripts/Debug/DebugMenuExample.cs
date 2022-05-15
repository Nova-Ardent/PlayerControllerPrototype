using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DebugMenu;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
