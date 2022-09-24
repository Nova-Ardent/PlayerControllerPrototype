using System.Collections;
using System.Collections.Generic;
using Utilities.Controller;
using Creatures.Player;
using UnityEngine;

namespace Objects.Interactable
{
    [RequireComponent(typeof(Collider))]
    public class InteractionPoint : MonoBehaviour
    {
        [SerializeField] Collider triggerCollider;
        [SerializeField] InteractableObject interactableObject;

        private void Awake()
        {
            if (triggerCollider == null)
            {
                triggerCollider = GetComponent<Collider>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.gameObject} has entered an interaction point.");
            if (other is CharacterController && other.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                playerController.CalloutController.SetCallouts(interactableObject.Interactions);
                InteractableUpdater.Instance.SetInteractableObject(interactableObject, playerController);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"{other.gameObject} has exited an interaction point.");
            if (other is CharacterController && other.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                playerController.CalloutController.ClearCallouts();
                InteractableUpdater.Instance.ClearCurrentObject();
            }
        }
    }
}