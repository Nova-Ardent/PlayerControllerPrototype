using Utilities.Common;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
namespace Utilities.Unity
{
    public class PropertyDrawerBuilder<T> : PropertyDrawer where T : class
    {
        public const int DEFAULT_INDEX_WIDTH = 18;
        public const int DEFAULT_INDEX_HEIGHT = 18;

        public struct Foldout
        {
            public bool foldout;
        }

        public T currentData;
        public Rect currentPosition;
        public SerializedProperty property;
        public GUIContent currentLabel;

        int initialHeight = 0;
        int finalHeight = 0;
        bool started = false;
        bool isDirty = false;

        public void StartGui(Rect position, SerializedProperty property, GUIContent label)
        {
            if (started)
            {
                Debug.LogWarning("GUI was never ended.");
            }

            started = true;
            EditorGUI.BeginProperty(position, label, property);

            if (currentData == null)
            {
                var obj = GetObjectWithPath(property.propertyPath.Split('.'), property.serializedObject.targetObject);
                if (obj != null && obj is T val)
                {
                    currentData = val;
                }
            }

            this.currentPosition = new Rect(position.x, position.y, position.width, DEFAULT_INDEX_HEIGHT);
            this.currentLabel = label;
            this.property = property;

            initialHeight = (int)currentPosition.y;
        }

        public void EndGui()
        {
            if (!started)
            {
                Debug.LogError("GUI was never started.");
            }

            started = false;
            finalHeight = (int)currentPosition.y;

            if (isDirty)
            {
                isDirty = false;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            EditorGUI.EndProperty();
        }

        public virtual void GenerateGUI()
        {

        }

        protected bool OnFoldout(ref Foldout foldout)
        {
            bool willFoldout = EditorGUI.Foldout(currentPosition, foldout.foldout, currentLabel);
            IndexPosition();

            foldout = new Foldout() { foldout = willFoldout };
            return foldout.foldout;
        }

        protected IDisposable Depth()
        {
            currentPosition = new Rect(currentPosition.x + DEFAULT_INDEX_WIDTH, currentPosition.y, currentPosition.width, currentPosition.height);

            return new DisposableAction(() =>
            {
                currentPosition = new Rect(currentPosition.x - DEFAULT_INDEX_WIDTH, currentPosition.y, currentPosition.width, currentPosition.height);
            });
        }

        public AnimationCurve CurveOption(string name, AnimationCurve curve, Rect range, int textWidth = -1, int graphWidth = -1, int graphHeight = -1)
        {
            Header(name, textWidth);
            if (graphWidth == -1)
            {
                graphWidth = (int)(currentPosition.width - textWidth - currentPosition.x);
            }

            if (graphHeight == -1)
            {
                graphHeight = (int)(currentPosition.width - textWidth - currentPosition.x);
            }

            Rect curveRect = new Rect(currentPosition.x, currentPosition.y, graphWidth, graphHeight);
            curve = EditorGUI.CurveField(curveRect, curve, Color.green, range);
            IncreaseHeight(graphHeight);

            return curve;
        }

        public E EnumOption<E>(string text, E val, int textWidth = 150, int enumWidth = -1) where E : Enum
        {
            if (enumWidth == -1)
            {
                enumWidth = (int)(currentPosition.width - textWidth - currentPosition.x);
            }

            Rect enumPosition = new Rect(currentPosition.x + textWidth, currentPosition.y, enumWidth, currentPosition.height);
            Rect labelPosition = new Rect(currentPosition.x, currentPosition.y, textWidth, currentPosition.height);

            EditorGUI.LabelField(labelPosition, text);
            if (EditorGUI.EnumPopup(enumPosition, val) is E result)
            {
                IndexPosition(2);
                if (!result.Equals(val))
                {
                    isDirty = true;
                }

                return result;
            }

            IndexPosition(2);
            return val;
        }

        public void Header(string text, int textWidth = -1)
        {
            if (textWidth == -1)
            {
                textWidth = (int)(currentPosition.width - currentPosition.x);
            }

            Rect labelPosition = new Rect(currentPosition.x, currentPosition.y, textWidth, currentPosition.height);

            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.fontStyle = FontStyle.Bold;

            EditorGUI.LabelField(labelPosition, text, style);

            IndexPosition();
        }

        public float SliderOption(string text, float value, float min, float max, int textWidth = 150, int sliderWidth = -1)
        {
            if (sliderWidth == -1)
            {
                sliderWidth = (int)(currentPosition.width - textWidth - currentPosition.x);
            }

            Rect sliderPosition = new Rect(currentPosition.x + textWidth, currentPosition.y, sliderWidth, currentPosition.height);
            Rect labelPosition = new Rect(currentPosition.x, currentPosition.y, textWidth, currentPosition.height);

            EditorGUI.LabelField(labelPosition, text);
            float newValue = EditorGUI.Slider(sliderPosition, value, min, max);
            if (newValue != value)
            {
                IndexPosition(2);
                
                isDirty = true;
                return newValue;
            }

            IndexPosition(2);
            return value;
        }

        public Vector3 Vector3Option(string text, Vector3 value, int vectorWidth = -1)
        {
            if (vectorWidth == -1)
            {
                vectorWidth = (int)(currentPosition.width - currentPosition.x);
            }

            Rect vectorPosition = new Rect(currentPosition.x, currentPosition.y, vectorWidth, currentPosition.height);

            Vector3 newValue = EditorGUI.Vector3Field(vectorPosition, text, value);
            if (!newValue.Equals(value))
            {
                value = newValue;
                isDirty = true;
            }
            
            IndexPosition(2);
            return value;
        }

        public void Space(int space = 1)
        {
            currentPosition = new Rect(currentPosition.x, currentPosition.y + (space * DEFAULT_INDEX_HEIGHT), currentPosition.width, currentPosition.height);
        }

        public void IndexPosition(int additive = 0)
        {
            currentPosition = new Rect(currentPosition.x, currentPosition.y + DEFAULT_INDEX_HEIGHT + additive, currentPosition.width, currentPosition.height);
        }

        public void IncreaseHeight(int additive = DEFAULT_INDEX_HEIGHT)
        {
            currentPosition = new Rect(currentPosition.x, currentPosition.y + additive, currentPosition.width, currentPosition.height);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            StartGui(position, property, label);

            GenerateGUI();

            EndGui();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return finalHeight - initialHeight;
        }

        object GetObjectWithPath(string[] path, object from, int depth = 0)
        {
            var children = from.GetType()
                .GetFields();
                
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].Name.Equals(path[depth]))
                {
                    if (depth == path.Length - 1)
                    {
                        return children[i].GetValue(from);
                    }
                    else
                    {
                        return GetObjectWithPath(path, children[i].GetValue(from), depth + 1);
                    }
                }
            }

            return null;
        }
    }
}
#endif