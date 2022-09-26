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
        [SerializeField] public GameObject characterHair;

        PlayerModelLoader.Characters currentCharacter = PlayerModelLoader.Characters.Masculine;
        public PlayerModelLoader.Characters CurrentCharacter
        {
            get => currentCharacter;
        }

        PlayerModelLoader.HairCuts currentHair = PlayerModelLoader.HairCuts.Bald;
        public PlayerModelLoader.HairCuts CurrentHair
        {
            get => currentHair;
        }

        public GameObject CharacterBody { get => characterBody; set => characterBody = value; }
        public GameObject CurrentEyebrows { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public GameObject CharacterHair { get => characterHair; set => characterHair = value; }

        public void EquipCharacter(PlayerModelLoader.Characters model)
        {
            currentCharacter = model;
            EquipCharacter(playerModelLoader.CharacterModels[model]);
        }

        public void EquipCharacter(GameObject gameObject)
        {
            if (characterBody != null)
            {
                GameObject.Destroy(characterBody);
            }
            characterBody = Equip(gameObject);
        }

        public void EquipEyebrows(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        public void EquipHair(PlayerModelLoader.HairCuts model)
        {
            currentHair = model;
            EquipHair(playerModelLoader.HaircutModels[model]);
        }

        public void EquipHair(GameObject gameObject)
        {
            if (characterHair != null)
            {
                GameObject.Destroy(characterHair);
            }
            characterHair = Equip(gameObject);
        }
    }
}