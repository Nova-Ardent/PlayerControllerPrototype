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
        public enum Debug
        {
            DebugPrintOne,
            DebugPrintTwo,
            DebugPrintThree,
            DebugPrintFour,
        }
    }
}

namespace Objects.Interactable
{
    public class TestInteractableObject : InteractableObject
    {
        [SerializeField] Material material;
        [SerializeField] Color[] colors;

        Interaction[] interactions = new Interaction[]
        {
            new Interaction() 
            {
                action = Localized.Debug.DebugPrintOne,
                control = Controller.Controls.InteractOne,
                callout = null,
            },
            new Interaction() 
            {
                action = Localized.Debug.DebugPrintTwo,
                control = Controller.Controls.InteractTwo,
                callout = null
            },
            new Interaction()
            {
                action = Localized.Debug.DebugPrintThree,
                control = Controller.Controls.InteractThree,
                callout = null
            },
            new Interaction()
            {
                action = Localized.Debug.DebugPrintFour,
                control = Controller.Controls.InteractFour,
                callout = null
            },
        };
        public override Interaction[] Interactions => interactions;

        public override void OnInteraction(Interaction interaction, PlayerController playerController)
        {
            if (interaction.control == Controller.Controls.InteractOne)
            {
                Debug.Log("one was pressed.");
                material.color = colors[0];
                return;
            }

            if (interaction.control == Controller.Controls.InteractTwo)
            {
                Debug.Log("two was pressed.");
                material.color = colors[1];
                return;
            }

            if (interaction.control == Controller.Controls.InteractThree)
            {
                Debug.Log("three was pressed.");
                material.color = colors[2];
                return;
            }

            if (interaction.control == Controller.Controls.InteractFour)
            {
                Debug.Log("four was pressed.");
                material.color = colors[3];
                return;
            }
        }
    }
}