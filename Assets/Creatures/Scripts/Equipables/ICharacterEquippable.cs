using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Creatures.Equippables
{
    public interface ICharacterEquippable<T> where T : Enum
    {
        public GameObject CharacterBody { get; set; }
        public void EquipCharacter(T bodyType);
        public void EquipCharacter(GameObject gameObject);
    }
}
