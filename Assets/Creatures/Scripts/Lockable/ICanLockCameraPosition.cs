using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.Player.Camera;

namespace Creatures.Lockable
{
    public interface ICanLockCameraPosition
    {
        public PlayerCameraFocalPoint FocusPointData { get; }
        public Transform CameraFocalPoint { get; }
    }
}