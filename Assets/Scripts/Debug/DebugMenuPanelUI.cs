using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DebugMenu
{
    public class DebugMenuPanel
    {
        public DebugMenuPanel(string title, DebugOption[] debugOptions)
        {
            this.title = new DebugMenuPanelTitle(title);
            this.debugOptions = debugOptions;
        }

        public void Append(IEnumerable<DebugOption> debugOptions)
        {
            this.debugOptions = this.debugOptions
                .Concat(debugOptions).ToArray();
        }

        public void RemoveOptionsWithParent(object parent)
        {
            debugOptions = debugOptions
                .Where(x => x.parent == parent)
                .ToArray();
        }

        public DebugMenuPanelTitle title;
        public DebugOption[] debugOptions;
    }

    public class DebugMenuPanelUI : MonoBehaviour
    {
        [System.Serializable]
        public class DebugOptions
        {
            public DebugOptionUI debugOptionUI;
        }

        [SerializeField] DebugMenuPanelPageUI page;
        [SerializeField] DebugMenuPanelTitleUI title;
        [SerializeField] DebugOptions debugOptions;

        float panelOffset = 0;
        DebugOptionUI currentOption;
        DebugOptionUI[] activeOptions;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            foreach (var option in activeOptions)
            {
                option.UpdateData();
            }
        }

        public void LoadPageData(DebugMenuPanel debugMenuPanel, int page, int totalPages)
        {
            panelOffset = 0;
            this.page.SetPage(page, totalPages);
            this.title.SetTitle(debugMenuPanel.title);
            GenerateOptions(debugMenuPanel.debugOptions);
        }

        public void GenerateOptions(DebugOption[] options)
        {
            if (activeOptions != null)
            {
                foreach (var option in activeOptions)
                {
                    Destroy(option.gameObject);
                }
            }

            activeOptions = options
                .Select(GenerateOption)
                .Select(OffsetUI)
                .ToArray();
            for (int i = 0; i < activeOptions.Length; i++)
            {
                if (i > 0)
                {
                    activeOptions[i].upOption = activeOptions[i - 1];
                }

                if (i < activeOptions.Length - 1)
                {
                    activeOptions[i].downOption = activeOptions[i + 1];
                }
            }

            if (activeOptions.Length > 0)
            {
                activeOptions[0].upOption = activeOptions[activeOptions.Length - 1];
                activeOptions[activeOptions.Length - 1].downOption = activeOptions[0];
                activeOptions.First().highlighted = true;
                currentOption = activeOptions.First();
            }
        }

        DebugOptionUI GenerateOption(DebugOption option)
        {
            DebugOptionUI debugOptionUI;
            if (option is DebugOption)
            {
                debugOptionUI = Instantiate(debugOptions.debugOptionUI, this.transform);
                debugOptionUI.ApplyData(option);
                return debugOptionUI;
            }

            Debug.LogError($"{option} - type doesn't exist");
            return Instantiate(debugOptions.debugOptionUI);
        }

        DebugOptionUI OffsetUI(DebugOptionUI option)
        {
            option.transform.position = option.transform.position + new Vector3(0, panelOffset, 0);
            if (option.transform is RectTransform rect)
            {
                panelOffset -= rect.sizeDelta.y;
            }
            return option;
        }

        public void UpPress()
        {
            if (currentOption == null || currentOption.upOption == null)
            {
                return;
            }

            currentOption.highlighted = false;
            currentOption = currentOption.upOption;
            currentOption.highlighted = true;
        }

        public void DownPress()
        {
            if (currentOption == null || currentOption.downOption == null)
            {
                return;
            }

            currentOption.highlighted = false;
            currentOption = currentOption.downOption;
            currentOption.highlighted = true;
        }
    }
}
