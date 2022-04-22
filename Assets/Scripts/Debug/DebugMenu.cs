using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugMenu
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] Callouts callouts;
        [SerializeField] GameObject mainDebugMenuPanel;

        public static Debug Instance
        {
            get;
            private set;
        }

        void Start()
        {
            if (Instance != null)
            {
                Debug.LogError("more than one debugmenu exists, destroying later instantiations.");
                Destroy(this.gameObject);
            }

            RegisterDefaultPanels();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if (Controller.debugMenuOpen)
            {
                if (Controller.GetKeyDown(Controller.Controls.DebugMenuClose) 
                    || Controller.GetKeyDown(Controller.Controls.DebugMenuOpen))
                {
                    Controller.debugMenuOpen = false;
                    mainDebugMenuPanel.gameObject.SetActive(false);
                }

                UpdateDebugMenu();
            }
            else
            {
                if (Controller.GetKeyDown(Controller.Controls.DebugMenuOpen))
                {
                    Controller.debugMenuOpen = true;
                    mainDebugMenuPanel.gameObject.SetActive(true);
                }
            }
        }

        void UpdateDebugMenu()
        {

        }

        void RegisterDefaultPanels()
        {

        }
    }
}