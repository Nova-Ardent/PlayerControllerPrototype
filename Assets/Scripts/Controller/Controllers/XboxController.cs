#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Controller;

public class XboxController : ControllerBase
{
    public override Controller.ControllerType controllerType => Controller.ControllerType.xbox;
}
