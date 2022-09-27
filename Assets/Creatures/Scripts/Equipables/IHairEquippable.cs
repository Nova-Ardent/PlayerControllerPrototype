using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    public interface IHairEquippable<T> where T : Enum
    {
        public GameObject CharacterHair { get; set; }
        public void EquipHair(T gameObject);
        public void EquipHair(GameObject gameObject);
    }
}