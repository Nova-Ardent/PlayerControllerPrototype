using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        static UIManager _Instance;
        public static UIManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    Debug.LogError("attempted to get get UI Manager before it's been instantiated.");
                    return null;
                }
                return _Instance;
            }
            set
            {
                if (_Instance != null)
                {
                    return;
                }
                _Instance = value;
            }
        }

        [SerializeField] Callouts callouts;

        [SerializeField] Camera camera;
        HighlightableButton currentButton = null;
        List<HighlightableButton> currentButtons = new List<HighlightableButton>();

        private void Start()
        {
            Localized.Instance.ValidateAndCreateLanguages();
            Localized.Instance.SetLanguage(0);

            if (callouts != null)
            {
                Controller.SetupControllers(callouts);
                Controller.SetControllerType(Controller.ControllerType.keyboard);
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (currentButtons == null || currentButtons.Count == 0)
            {
                return;
            }

            if (Controller.GetAxis(Controller.Controls.MouseVertical) != 0 
             || Controller.GetAxis(Controller.Controls.MouseHorizontal) != 0)
            {
                UnhighlightCurrent();
            }

            if (currentButton == null)
            {
                if (Controller.GetKeyDown(Controller.Controls.MenuNavUp)
                 || Controller.GetKeyDown(Controller.Controls.MenuNavLeft)
                 || Controller.GetKeyDown(Controller.Controls.MenuNavRight)
                 || Controller.GetKeyDown(Controller.Controls.MenuNavDown))
                {
                    currentButton = currentButtons[0];
                    foreach (var button in currentButtons)
                    {
                        if (button.isDefaultButton)
                        {
                            HighlightButton(button);
                            break;
                        }
                    }

                    return;
                }
            }

            OnMenuNavPress();
        }

        public void OnMenuNavPress()
        {
            if (Controller.GetKeyDown(Controller.Controls.MenuNavUp)) OnPress(currentButton.upOverride, currentButton.navUp);
            if (Controller.GetKeyDown(Controller.Controls.MenuNavRight)) OnPress(currentButton.rightOverride, currentButton.navLeft);
            if (Controller.GetKeyDown(Controller.Controls.MenuNavLeft)) OnPress(currentButton.leftOverride, currentButton.navRight);
            if (Controller.GetKeyDown(Controller.Controls.MenuNavDown)) OnPress(currentButton.downOverride, currentButton.navDown);

            if (Controller.GetKeyUp(Controller.Controls.MenuNavUp)) OnDepress(currentButton.upOverride);
            if (Controller.GetKeyUp(Controller.Controls.MenuNavRight)) OnDepress(currentButton.rightOverride);
            if (Controller.GetKeyUp(Controller.Controls.MenuNavLeft)) OnDepress(currentButton.leftOverride);
            if (Controller.GetKeyUp(Controller.Controls.MenuNavDown)) OnDepress(currentButton.downOverride);
        }

        public void OnPress(HighlightableButton.ButtonPressOverride pressOverride, HighlightableButton menuNav)
        {
            if (pressOverride.hasButton && pressOverride.button.interactable)
            {
                pressOverride.button.onClick.Invoke();
                pressOverride.buttonAnim.SetTrigger("Pressed");
            }
            else
            {
                HighlightButton(menuNav);
            }
        }

        public void OnDepress(HighlightableButton.ButtonPressOverride pressOverride)
        {
            if (!pressOverride.hasButton)
            {
                return;
            }

            if (currentButton.rightOverride.button.interactable)
            {
                pressOverride.buttonAnim.SetTrigger("Normal");
            }
            else
            {
                pressOverride.buttonAnim.SetTrigger("Disabled");
            }
        }

        public void UnhighlightCurrent()
        {
            if (currentButton != null)
            {
                currentButton.buttonAnim.SetTrigger("Normal");
                currentButton = null;
            }
        }

        public void HighlightButton(HighlightableButton button)
        {
            if (button == null)
            {
                return;
            }

            UnhighlightCurrent();
            button.buttonAnim.SetTrigger("Highlighted");
            currentButton = button;
        }

        public void Register(HighlightableButton highlightable)
        {
            UnhighlightCurrent();
            currentButtons.Add(highlightable);
        }

        public void Unregister(HighlightableButton highlightable)
        {
            UnhighlightCurrent();
            currentButtons.Remove(highlightable);
        }
    }
}
