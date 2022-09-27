using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    public interface IEyesEquippable<T> where T : Enum
    {
        public GameObject CharacterEyes { get; set; }
        public void EquipEyes(T gameObject);
        public void EquipEyes(GameObject gameObject);
    }
}