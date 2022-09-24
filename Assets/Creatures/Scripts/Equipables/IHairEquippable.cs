using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Equippables
{
    public interface IHairEquippable
    {
        public GameObject CurrentHair { get; set; }
        public void EquipHair(GameObject gameObject);
    }
}