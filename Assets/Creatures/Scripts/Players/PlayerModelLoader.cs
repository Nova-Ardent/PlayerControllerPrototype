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

        public enum HairCuts
        {
            Bald,
            BluntCut,
            Bun,
            Clown,
            Curly,
            Fohawk,
            Hightop,
            IveLeague,
            LeftComb,
            LibertyHawk,
            LongCut,
            LowTop,
            MidCut,
            PonytailLong,
            RightComb,
            ShorthairBangs,
            SimpleCut,
            SpikeHawk,
            Tendrils,
            TendrilsWPonyTail,
        }

        public DataMap<Characters, GameObject> CharacterModels;
        public DataMap<HairCuts, GameObject> HaircutModels;
    }
}
