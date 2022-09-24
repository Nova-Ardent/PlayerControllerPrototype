using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities.Controller.Controller;

namespace Utilities.Controller
{
    public class Keyboard : ControllerBase
    {
        public override ControllerType controllerType => ControllerType.keyboard;
    }
}