using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class Localized
{
    public enum MenuAction
    {
        None,
        Accept,
        Cancel,
    }
}

namespace UI
{
    public class HighlightableButton : MonoBehaviour
    {
        [System.Serializable]
        public struct ButtonPressOverride
        {
            public Animator buttonAnim;
            public Button button;
            public bool hasButton => button != null;
        }

        public bool isDefaultButton;
        public Animator buttonAnim;

        public HighlightableButton navUp;
        public HighlightableButton navLeft;
        public HighlightableButton navRight;
        public HighlightableButton navDown;

        public ButtonPressOverride upOverride;
        public ButtonPressOverride leftOverride;
        public ButtonPressOverride rightOverride;
        public ButtonPressOverride downOverride;

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            UIManager.Instance.Unregister(this);
        }
    }
}
