using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        static UIManager _Instance;
        public static UIManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    Debug.LogError("attempted to get get UI Manager before it's been instantiated.");
                    return null;
                }
                return _Instance;
            }
            set
            {
                if (_Instance == null)
                {
                    return;
                }
                _Instance = value;
            }
        }



        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Register(Highlightable highlightable)
        {

        }

        public void Unregister(Highlightable highlightable)
        {

        }
    }
}
