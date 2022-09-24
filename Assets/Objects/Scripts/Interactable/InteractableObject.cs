using System.Collections;
using System.Collections.Generic;
using Utilities.Controller;
using Creatures.Player;
using UnityEngine;

namespace Objects.Interactable
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public abstract Interaction[] Interactions { get; }
        public abstract void OnInteraction(Interaction interaction, PlayerController playerController);

        public virtual void UpdateInteractable(PlayerController playerController)
        {
            for (int i = 0; i < Interactions.Length; i++)
            {
                if (Controller.GetKeyDown(Interactions[i].control))
                {
                    OnInteraction(Interactions[i], playerController);
                }
            }
        }
    }
}