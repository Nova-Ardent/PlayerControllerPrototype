using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

namespace Creatures
{
    public interface ILockable
    {
        bool IsLocked { get; }
        void Lock(InteractableObject lockedBy);
        void Unlock();
    }
}
