using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Creatures.Player;
using Creatures.Equippables;

namespace Creatures.Player
{
    [Serializable]
    public class PlayerEquippable : Equippable
        , ICharacterEquippable
        , IEyebrowsEquippable
        , IHairEquippable
    {
        [SerializeField] public PlayerModelLoader playerModelLoader;
        [SerializeField] public GameObject characterBody;

        PlayerModelLoader.Characters currentCharacter = PlayerModelLoader.Characters.Masculine;
        public PlayerModelLoader.Characters CurrentCharacter
        {
            get => currentCharacter;
        }

        public GameObject CharacterBody { get => characterBody; set => characterBody = value; }
        public GameObject CurrentEyebrows { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public GameObject CurrentHair { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void EquipCharacter(PlayerModelLoader.Characters model)
        {
            currentCharacter = model;
            EquipCharacter(playerModelLoader.CharacterModels[model]);
        }

        public void EquipCharacter(GameObject gameObject)
        {
            GameObject.Destroy(characterBody);
            characterBody = Equip(gameObject);
        }

        public void EquipEyebrows(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        public void EquipHair(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }
    }
}