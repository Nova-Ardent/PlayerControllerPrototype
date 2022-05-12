using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace DebugMenu
{
    public class DebugOption
    {
        public DebugOption(string name, string description)
        {
            this.name = name;
            this.description = () => description;
        }

        public DebugOption(string name, Func<string> description)
        {
            this.name = name;
            this.description = description;
        }

        public DebugOption SetParent(object parent)
        {
            this.parent = parent;
            return this;
        }

        public string name;
        public Func<string> description;
        public object parent;
    }

    public class DebugOptionUI : MonoBehaviour
    {
        protected bool _highlighted;
        public bool highlighted
        {
            get => _highlighted;
            set
            {
                _highlighted = value;
                backing.color = value ? backingHighlighted : backingRegular;
            }
        }

        private DebugOption debugOption;
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

        public virtual void UpdateData()
        {
            title.text = debugOption.name;
            description.text = debugOption.description();
        }

        public void ApplyData(DebugOption debugOption)
        {
            this.debugOption = debugOption;
        }
    }
}
