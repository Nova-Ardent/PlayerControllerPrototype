using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Controller;
using Utilities.UI;
using Utilities.Localization;
using Creatures.Player;

namespace Utilities.Localization
{
    public partial class Localized
    {
        public enum WardrobeSelections
        {
            Hair,
            Beard,
            Character,
        }
    }
}

namespace Objects.Interactable
{
    public class WardrobeObject : InteractableObject
    {
        Interaction[] interactions = new Interaction[]
        {
            new Interaction()
            {
                action = Localized.WardrobeSelections.Hair,
                control = Controller.Controls.InteractOne,
                callout = null,
            },
            new Interaction()
            {
                action = Localized.WardrobeSelections.Beard,
                control = Controller.Controls.InteractTwo,
                callout = null
            },
            new Interaction()
            {
                action = Localized.WardrobeSelections.Character,
                control = Controller.Controls.InteractThree,
                callout = null
            },
        };
        public override Interaction[] Interactions => interactions;

        public override void OnInteraction(Interaction interaction, PlayerController playerController)
        {
            if (interaction.control == Controller.Controls.InteractOne)
            {
                
            }
            else if (interaction.control == Controller.Controls.InteractTwo)
            {
                
            }
            else if (interaction.control == Controller.Controls.InteractThree)
            {
                switch (playerController.PlayerEquippable.CurrentCharacter)
                {
                    case PlayerModelLoader.Characters.Masculine:
                        playerController.PlayerEquippable.EquipCharacter(PlayerModelLoader.Characters.Feminine);
                        break;
                    case PlayerModelLoader.Characters.Feminine:
                        playerController.PlayerEquippable.EquipCharacter(PlayerModelLoader.Characters.Masculine);
                        break;
                }
            }
        }
    }
}