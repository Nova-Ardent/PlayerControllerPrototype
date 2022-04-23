using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DebugMenu
{
    public class DebugMenuPanelPageUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI pageText;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetPage(int page, int ofPage)
        {
            pageText.text = $"page {page}/{ofPage}";
        }
    }
}
