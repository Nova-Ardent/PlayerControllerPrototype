using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DebugMenu
{
    public class DebugMenuPanelTitle
    {
        public DebugMenuPanelTitle(string text)
        {
            this.text = text;
        }

        public string text;
    }

    public class DebugMenuPanelTitleUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetTitle(DebugMenuPanelTitle title)
        {
            this.title.text = title.text;
        }
    }
}
