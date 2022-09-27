using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    public interface IEyebrowsEquippable<T> where T : Enum
    {
        public GameObject CharacterEyebrows { get; set; }
        public void EquipEyebrows(T gameObject);
        public void EquipEyebrows(GameObject gameObject);
    }
}