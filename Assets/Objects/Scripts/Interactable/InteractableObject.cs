using UnityEngine;
using Utilities.Controller;
using Creatures.Player;
using Creatures.Player.Camera;
using Objects.UI;

namespace Objects.Interactable
{
    public abstract class InteractableObject : MonoBehaviour
    {
        protected PlayerMovement playerMovement;
        protected PlayerCameraController playerCameraController;
        protected CalloutController calloutController;

        public abstract Interaction[] Interactions { get; }
        public abstract void OnInteraction(Interaction interaction, PlayerController playerController);
        public virtual void OnUpdate(PlayerController playerController)
        {
            
        }

        public virtual void UpdateInteractable(PlayerController playerController)
        {
            for (int i = 0; i < Interactions.Length; i++)
            {
                if (Controller.GetKeyDown(Interactions[i].control))
                {
                    OnInteraction(Interactions[i], playerController);
                }
            }

            OnUpdate(playerController);
        }

        public void LockPlayer(PlayerController playerController)
        {
            playerMovement = playerController.PlayerMovement;
            playerCameraController = playerController.PlayerCameraController;
            calloutController = playerController.CalloutController;

            playerMovement.Lock(this);
            calloutController.Lock(this);
            playerCameraController.Lock(this);
        }

        public void UnlockPlayer(PlayerController playerController)
        {
            playerMovement.Unlock(this);
            calloutController.Unlock(this);
            playerCameraController.Unlock(this);
            
            playerMovement = null;
            playerCameraController = null;
            calloutController = null;
        }
    }
}