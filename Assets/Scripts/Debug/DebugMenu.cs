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
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!Controller.controllersAreSetup)
            {
                Controller.SetupControllers(callouts);
                Controller.SetControllerType(Controller.ControllerType.keyboard);
            }

            debugMenuPanels = new DebugMenuPanel[0];

            RegisterDefaultPanels();
#endif
            if (Instance != null)
            {
                Debug.LogWarning("more than one debugmenu exists, destroying later instantiations.");
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
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
            RegisterPanel("Default Panel", this
            , new DebugOption("boop", "boop")
            );
        }

        public void RegisterPanel(string title, params DebugOption[] debugOptions)
        {
            RegisterPanel(title, null, debugOptions);
        }

        public void RegisterPanel(string title, object from, params DebugOption[] debugOptions)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (debugMenuPanels.Any(x => x.title.Equals(title), out int index))
            {
                debugMenuPanels[index]
                    .Append(debugOptions.Select(x => x.SetParent(from)));
            }

            debugMenuPanels = debugMenuPanels
                .Append(new DebugMenuPanel(title, debugOptions))
                .ToArray();
#endif
        }

        public void UnRegisterPanel(string title)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            debugMenuPanels = debugMenuPanels
                .Where(x => !x.Equals(title))
                .ToArray();
#endif
        }

        public void UnRegisterPanel(string title, object from)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
#endif
        }
    }
}