using System.Collections;
using System.Collections.Generic;
using Creatures.Player;
using UnityEngine;

namespace Objects.Interactable
{
    public class InteractableUpdater : MonoBehaviour
    {
        InteractableObject currentObject;
        PlayerController playerController;
        public static InteractableUpdater Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("there is more than one instance of InteractableUpdater.");
            }
            Instance = this;
        }

        private void Update()
        {
            this.currentObject?.UpdateInteractable(playerController);
        }

        public void SetInteractableObject(InteractableObject currentObject, PlayerController playerController)
        {
            if (this.currentObject != null || this.playerController != null)
            {
                return;
            }

            this.currentObject = currentObject;
            this.playerController = playerController;
        }

        public void ClearCurrentObject()
        {
            this.currentObject = null;
            this.playerController = null;
        }
    }
}
