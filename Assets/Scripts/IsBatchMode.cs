using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBatchMode : MonoBehaviour
{
    void Start()
    {
        if (Application.isBatchMode)
        {
            Debug.LogError("running in server mode:");
        }
        else
        {
            Debug.LogError($"running in client mode: grpahics card {SystemInfo.graphicsDeviceName}");
        }
    }
}
