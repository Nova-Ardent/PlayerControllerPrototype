using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Equippables
{
    public interface IHairEquippable
    {
        public GameObject CharacterHair { get; set; }
        public void EquipHair(GameObject gameObject);
    }
}