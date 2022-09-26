using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Lockable
{
    public interface ICanLockPlayerPosition
    {
        public Transform StandingPoint { get; }
    }
}