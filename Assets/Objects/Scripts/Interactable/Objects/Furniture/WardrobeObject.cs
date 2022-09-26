using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Controller;
using Utilities.UI;
using Utilities.Localization;
using Creatures.Lockable;
using Creatures.Player;
using Creatures.Player.Camera;
using Objects.UI;
using System;

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
        , ICanLockPlayerPosition
        , ICanControlCallouts
        , ICanLockCameraPosition
    {
        public enum Mode
        {
            Unfocused,
            ChangingHair,
            ChangingBeard,
        }

        [SerializeField] Transform standingPoint;

        [SerializeField] PlayerCameraFocalPoint focusPointData;
        [SerializeField] Transform cameraFocalPoint;

        [SerializeField] SelectorRing hairSelectorRing;
        [SerializeField] SelectorRing beardSelectorRing;

        public Transform StandingPoint
        {
            get => standingPoint;
        }

        public Transform CameraFocalPoint
        {
            get => cameraFocalPoint;
        }

        public PlayerCameraFocalPoint FocusPointData
        {
            get => focusPointData;
        }
        
        private Mode mode;

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
            if (mode == Mode.Unfocused)
            {   
                UnFocused(interaction, playerController);
            }
        }

        public override void OnUpdate(PlayerController playerController)
        {
            switch (mode)
            {
                case Mode.ChangingHair:
                    ChangingHair(playerController);
                    break;
                case Mode.ChangingBeard:
                    ChangingBeard(playerController);
                    break;
            }
        }

        private void UnFocused(Interaction interaction, PlayerController playerController)
        {
            if (interaction.control == Controller.Controls.InteractOne)
            {
                LockPlayer(playerController);

                hairSelectorRing.Initialize();
                mode = Mode.ChangingHair;
            }
            else if (interaction.control == Controller.Controls.InteractTwo)
            {
                LockPlayer(playerController);

                beardSelectorRing.Initialize();
                mode = Mode.ChangingBeard;
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

        private void ChangingHair(PlayerController playerController)
        {
            if (Controller.GetKeyDown(Controller.Controls.Cancel))
            {
                UnlockPlayer(playerController);
                hairSelectorRing.Deinitialize();
                mode = Mode.Unfocused;
            }
            else if (Controller.GetKeyDown(Controller.Controls.ChangeSelectionLeft))
            {
                hairSelectorRing.Increment();
                if (Enum.IsDefined(typeof(PlayerModelLoader.HairCuts), hairSelectorRing.Index))
                {
                    playerController.PlayerEquippable.EquipHair((PlayerModelLoader.HairCuts)hairSelectorRing.Index);
                }
            }
            else if (Controller.GetKeyDown(Controller.Controls.ChangeSelectionRight))
            {
                hairSelectorRing.Decrement();
                if (Enum.IsDefined(typeof(PlayerModelLoader.HairCuts), hairSelectorRing.Index))
                {
                    playerController.PlayerEquippable.EquipHair((PlayerModelLoader.HairCuts)hairSelectorRing.Index);
                }
            }
            hairSelectorRing.UpdateRotation();
        }

        public void ChangingBeard(PlayerController playerController)
        {
            if (Controller.GetKeyDown(Controller.Controls.Cancel))
            {
                UnlockPlayer(playerController);
                beardSelectorRing.Deinitialize();
                mode = Mode.Unfocused;
            }
        }
    }
}