using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities.Controller.Controller;

namespace Utilities.Controller
{
    public class XboxController : ControllerBase
    {
        public override Controller.ControllerType controllerType => Controller.ControllerType.xbox;
    }
}
