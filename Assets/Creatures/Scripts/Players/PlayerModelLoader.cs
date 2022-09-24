using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Unity;

namespace Creatures.Player
{
    [CreateAssetMenu(fileName = "PlayerModelLoader", menuName = "Creatures/ModelLoader/PlayerModelLoader")]
    public class PlayerModelLoader : ScriptableObject
    {
        public enum Characters
        {
            Masculine,
            Feminine,
        }

        public DataMap<Characters, GameObject> CharacterModels;
    }
}
