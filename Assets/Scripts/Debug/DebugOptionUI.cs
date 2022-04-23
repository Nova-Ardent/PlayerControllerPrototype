using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DebugMenu
{
    public class DebugOption
    {
        public DebugOption(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string name;
        public string description;
    }

    public class DebugOptionUI : MonoBehaviour
    {
        bool _highlighted;
        public bool highlighted
        {
            get => _highlighted;
            set
            {
                _highlighted = value;
                backing.color = value ? backingHighlighted : backingRegular;
            }
        }

        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        public Image backing;
        public Color backingRegular;
        public Color backingHighlighted;

        // will implement left and right when we have enough options to matter.
        public DebugOptionUI upOption;
        //public DebugOptionUI rightOption;
        public DebugOptionUI downOption;
        //public DebugOptionUI leftOption;

        public void ApplyData(DebugOption debugOption)
        {
            title.text = debugOption.name;
            description.text = debugOption.description;
        }
    }
}
