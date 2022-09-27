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
        , ICharacterEquippable<PlayerModelLoader.Characters>
        , IEyebrowsEquippable<PlayerModelLoader.Characters>
        , IEyesEquippable<PlayerModelLoader.Characters>
        , IHairEquippable<PlayerModelLoader.HairCuts>
        , IBeardEquippable<PlayerModelLoader.Beards>
    {
        [SerializeField] public PlayerModelLoader playerModelLoader;
        [SerializeField] public GameObject characterBody;
        [SerializeField] public GameObject characterEyebrows;
        [SerializeField] public GameObject characterEyes;
        [SerializeField] public GameObject characterHair;
        [SerializeField] public GameObject characterBeard;

        PlayerModelLoader.Characters currentCharacter = PlayerModelLoader.Characters.Masculine;
        public PlayerModelLoader.Characters CurrentCharacter
        {
            get => currentCharacter;
        }

        PlayerModelLoader.Characters currentEyebrows = PlayerModelLoader.Characters.Masculine;
        public PlayerModelLoader.Characters CurrentEyebrows
        {
            get => currentEyebrows;
        }

        public PlayerModelLoader.Characters currentEyes = PlayerModelLoader.Characters.Masculine;
        public PlayerModelLoader.Characters CurrentEyes
        {
            get => currentEyes;
        }

        PlayerModelLoader.HairCuts currentHair = PlayerModelLoader.HairCuts.Bald;
        public PlayerModelLoader.HairCuts CurrentHair
        {
            get => currentHair;
        }

        public PlayerModelLoader.Beards currentBeard = PlayerModelLoader.Beards.Beardless;
        public PlayerModelLoader.Beards CurrentBeard
        {
            get => currentBeard;
        }

        public GameObject CharacterBody { get => characterBody; set => characterBody = value; }
        public GameObject CharacterEyebrows { get => characterEyebrows; set => characterEyebrows = value; }
        public GameObject CharacterEyes { get => characterEyes; set => characterEyes = value; }
        public GameObject CharacterHair { get => characterHair; set => characterHair = value; }
        public GameObject CharacterBeard { get => characterBeard; set => characterBeard = value; }

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

        public void EquipEyebrows(PlayerModelLoader.Characters model)
        {
            currentEyebrows = model;
            EquipEyebrows(playerModelLoader.CharacterEyebrows[model]);
        }

        public void EquipEyebrows(GameObject gameObject)
        {
            if (characterEyebrows != null)
            {
                GameObject.Destroy(characterEyebrows);
            }
            characterEyebrows = Equip(gameObject);
        }

        public void EquipEyes(PlayerModelLoader.Characters model)
        {
            currentEyes = model;
            EquipEyes(playerModelLoader.CharacterEyes[model]);
        }

        public void EquipEyes(GameObject gameObject)
        {
            if (characterEyes != null)
            {
                GameObject.Destroy(characterEyes);
            }
            characterEyes = Equip(gameObject);
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

        public void EquipBeard(PlayerModelLoader.Beards beards)
        {
            currentBeard = beards;
            EquipBeard(playerModelLoader.BeardModels[beards]);
        }

        public void EquipBeard(GameObject gameObject)
        {
            if (characterBeard != null)
            {
                GameObject.Destroy(characterBeard);
            }
            characterBeard = Equip(gameObject);
        }
    }
}