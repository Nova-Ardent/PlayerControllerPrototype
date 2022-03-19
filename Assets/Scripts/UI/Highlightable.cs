using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    public class Highlightable : MonoBehaviour
    {
        public Action onHighlight { get; private set; }
        public Action onUnhighlight { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.Register(this);
        }

        private void OnDestroy()
        {
            UIManager.Instance.Unregister(this);
        }

        public void RegisterCallback(Action onHighlight, Action onUnhighlight)
        {
            this.onHighlight = onHighlight;
            this.onUnhighlight = onUnhighlight;
        }

        public void UnregisterCallbacks(Action onHighlight, Action onUnhighlight)
        {

        }
    }
}
