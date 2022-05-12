using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugMenuExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string lastPressed = "press me";

        DebugMenu.DebugMenu.Instance.RegisterPanel
            ( "debug menu example script"
            , new DebugMenu.DebugOption("static name", "static description")
            , new DebugMenu.DebugOption("static name", () => $"dynamic text: {DateTime.Now}")
            , new DebugMenu.DebugOptionAction("Action", () => lastPressed, () => lastPressed = $"last pressed: {DateTime.Now}")
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
