using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Controller;
using static Utilities.Controller.Controller;

namespace Utilities.Controller
{
    public class ControllerInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (!Controller.ControllerInitialized)
            {
                Controller.SetupControllers();
                Controller.SetControllerType(ControllerType.keyboard);
            }
        }
    }
}
