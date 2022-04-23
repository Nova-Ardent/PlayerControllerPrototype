using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DebugMenu
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] Callouts callouts;
        [SerializeField] GameObject mainDebugMenuPanel;
        [SerializeField] DebugMenuPanelUI debugMenuPanelUI;
        
        DebugMenuPanel[] debugMenuPanels;
        int currentPage;

        public static DebugMenu Instance
        {
            get;
            private set;
        }

        void Start()
        {
            if (!Controller.controllersAreSetup)
            {
                Controller.SetupControllers(callouts);
                Controller.SetControllerType(Controller.ControllerType.keyboard);
            }

            if (Instance != null)
            {
                Debug.LogWarning("more than one debugmenu exists, destroying later instantiations.");
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            debugMenuPanels = new DebugMenuPanel[0];
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

                    debugMenuPanelUI.LoadPageData(debugMenuPanels.First(), currentPage, debugMenuPanels.Length);
                }
            }
        }

        void UpdateDebugMenu()
        {
            if (Controller.GetKeyDown(Controller.Controls.DebugPageUp))
            {
                currentPage++;
                if (currentPage == debugMenuPanels.Length)
                {
                    currentPage = 0;
                }
                debugMenuPanelUI.LoadPageData(debugMenuPanels[currentPage], currentPage, debugMenuPanels.Length);
            }
            else if (Controller.GetKeyDown(Controller.Controls.DebugPageDown))
            {
                currentPage--;
                if (currentPage < 0)
                {
                    currentPage = debugMenuPanels.Length - 1;
                }
                debugMenuPanelUI.LoadPageData(debugMenuPanels[currentPage], currentPage, debugMenuPanels.Length);
            }

            if (Controller.GetKeyDown(Controller.Controls.DebugDown))
            {
                debugMenuPanelUI.DownPress();
            }
            if (Controller.GetKeyDown(Controller.Controls.DebugUp))
            {
                debugMenuPanelUI.UpPress();
            }
        }

        void RegisterDefaultPanels()
        {
            RegisterPanel("Default Panel");
        }

        public void RegisterPanel(string title, params DebugOption[] debugOptions)
        {
            debugMenuPanels = debugMenuPanels
                .Append(new DebugMenuPanel(title, debugOptions))
                .ToArray();
        }

        public void UnRegisterPanel(string title)
        {
            debugMenuPanels = debugMenuPanels
                .Where(x => !x.Equals(title))
                .ToArray();
        }
    }
}