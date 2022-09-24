using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities.Controller.Controller;

namespace Utilities.Controller
{
    public class PSController : ControllerBase
    {
        public override Controller.ControllerType controllerType => Controller.ControllerType.ps;
    }
}