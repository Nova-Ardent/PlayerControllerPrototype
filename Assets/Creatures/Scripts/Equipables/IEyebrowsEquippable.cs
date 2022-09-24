using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Equippables
{
    public interface IEyebrowsEquippable
    {
        public GameObject CurrentEyebrows { get; set; }
        public void EquipEyebrows(GameObject gameObject);
    }
}