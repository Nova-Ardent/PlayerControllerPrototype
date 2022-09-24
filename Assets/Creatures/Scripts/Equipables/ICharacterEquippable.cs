using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Equippables
{
    public interface ICharacterEquippable
    {
        public GameObject CharacterBody { get; set; }
        public void EquipCharacter(GameObject gameObject);
    }
}
