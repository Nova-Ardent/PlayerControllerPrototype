using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Objects
{
    public class SelectorRing : MonoBehaviour
    {
        public enum PositionMode
        {
            Center,
            OffsetToSelected,
        }

        bool initialized = false;
        float circumference = 0;
        float radius;
        Quaternion currentRotation;
        GameObject[] selections;

        [SerializeField] PositionMode positionMode;
        [SerializeField] float scale = 1;
        [SerializeField] int index = 0;
        [SerializeField] public GameObject[] prefabs;
        [SerializeField] public bool initializeInStart;
#if UNITY_EDITOR
        [SerializeField] public bool isUsingDebugObjects;
#endif

        public int Index
        {
            get => index;
        }

        private void Start()
        {
            if (initializeInStart)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            InstantiatePrefabs();
            InitializeRing();

            if (prefabs.Length == 0)
            {
                Debug.LogError($"{this.gameObject.name} is a selector with 0 prefabs, and isn't useful and will be destroyed.");
                Destroy(this.gameObject);
            }
        }

        public void Deinitialize()
        {
            if (positionMode == PositionMode.OffsetToSelected)
            {
                this.transform.localPosition -= new Vector3(0, 0, -radius);
            }

            initialized = false;
            circumference = 0;
            radius = 0;
            Quaternion currentRotation = Quaternion.identity;

            for (int i = 0; i < selections.Length; i++)
            {
                Destroy(selections[i]);
            }
        }

        void InstantiatePrefabs()
        {
            selections = new GameObject[prefabs.Length];
            for (int i = 0; i < prefabs.Length; i++)
            {
                selections[i] = Instantiate(prefabs[i], this.transform);
#if UNITY_EDITOR
                if (isUsingDebugObjects && selections[i].TryGetComponent<Renderer>(out Renderer render))
                {
                    float c = i * 1.0f / prefabs.Length;
                    render.material.color = new Color(c, c, c);
                }
#endif
            }
        }

        void InitializeRing()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            if (selections.Length == 0)
            {
                return;
            }

            circumference = selections.Length * scale;
            radius = circumference / (2 * Mathf.PI);
            for (int i = 0; i < selections.Length; i++)
            {
                selections[i].transform.SetParent(this.transform);
                selections[i].transform.localPosition = new Vector3(
                    radius * Mathf.Sin((i * 360.0f / selections.Length).DegreesToRads()),
                    0,
                    radius * Mathf.Cos((i * 360.0f / selections.Length).DegreesToRads()));
                selections[i].transform.localRotation = Quaternion.Euler(0, i * 360.0f / selections.Length, 0);
            }

            if (positionMode == PositionMode.OffsetToSelected)
            {
                this.transform.localPosition += new Vector3(0, 0, -radius);
            }
        }

        public void UpdateRotation()
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, currentRotation, 0.05f);
        }

        public void Increment()
        {
            index = index + 1;
            if (index > selections.Length - 1)
            {
                index = 0;
            }

            SetCurrentRotation();
        }

        public void Decrement()
        {
            index = index - 1;
            if (index < 0)
            {
                index = selections.Length - 1;
            }

            SetCurrentRotation();
        }
        
        public void SetIndex(int index)
        {
            this.index = index;
            SetCurrentRotation();
        }

        public void SetCurrentRotation()
        {
            currentRotation = Quaternion.Euler(0, -Index * 360.0f / selections.Length, 0);
        }
    }
}