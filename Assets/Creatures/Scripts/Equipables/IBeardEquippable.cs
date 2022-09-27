using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    public interface IBeardEquippable<T> where T : Enum
    {
        public GameObject CharacterBeard { get; set; }
        public void EquipBeard(T gameObject);
        public void EquipBeard(GameObject gameObject);
    }
}
