#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[Serializable]
public abstract class DataMap
{
    public abstract IEnumerable<string> GetLabels();
    public abstract int GetNumLabels();
    public abstract void UpdateEnumList();
}


[Serializable]
public class DataMap<K, V> : DataMap
    where K : Enum
    where V : UnityEngine.Object
{
    Dictionary<K, int>? _keyRefs;
    Dictionary<K, int> keyRefs 
    { 
        get 
        {
            if (_keyRefs != null && _keyRefs.Count != 0)
            {
                return _keyRefs;
            }

            _keyRefs = new Dictionary<K, int>();

            int i = 0;
            foreach (var key in keys)
            {
                _keyRefs[key] = i;
                i++;
            }
            return _keyRefs;
        }
        set
        {
            _keyRefs = value;
        }
    }

    public K[] keys;
    public V[] values;

    public DataMap()
    {
        keys = Utilities.GetEnums<K>().ToArray();
        values = new V[keys.Length];
    }

    public override IEnumerable<string> GetLabels()
    {
        return keys.Select(x => x.ToString());
    }

    public override int GetNumLabels()
    {
        return keys.Length;
    }

    public override void UpdateEnumList()
    {
        // need to update this function so that it takes the keys names, and not their value when the enum
        // gets updated.
        var tempKeys = keys;
        var tempValues = values;

        keys = Utilities.GetEnums<K>().ToArray();
        values = new V[keys.Length];
        _keyRefs = new Dictionary<K, int>();

        int i = 0;
        foreach (var key in keys)
        {
            _keyRefs[key] = i;
            i++;
        }

        for (int j = 0; j < tempKeys.Length; j++)
        {
            if (!Enum.IsDefined(typeof(K), tempKeys[j]))
            {
                continue;
            }

            if (_keyRefs.ContainsKey(tempKeys[j]))
            {
                values[_keyRefs[tempKeys[j]]] = tempValues[j];
            }
        }
    }

    public V GetV(int i)
    {
        return values[i];
    }

    public V GetV(K i)
    { 
        return values[keyRefs[i]];
    }

    public V? GetV(Enum i)
    {
        if (i is K k)
        {
            return values[keyRefs[k]];
        }

        Debug.LogError($"Enum value is invalid type for ${i}");
        return null;
    }

    public V this[int i]
    {
        get => GetV(i);
    }

    public V this[K i]
    {
        get => GetV(i);
    }

    public V? this[Enum i]
    {
        get => GetV(i);
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(DataMap<,>))]
public class DataMapEditor : PropertyDrawer
{
    bool foldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 18), foldout, label);

        if (foldout)
        {
            var value = fieldInfo.GetValue(property.serializedObject.targetObject) as DataMap;
            if (value != null)
            {
                SerializedProperty serializedValues = property.FindPropertyRelative("values");

                int i = 0;
                foreach (var valueLabels in value.GetLabels())
                {
                    position = new Rect(25, position.y + 18, position.width, 18);

                    EditorGUI.LabelField(position, valueLabels);

                    var serializedPosition = new Rect(150, position.y, position.width - 140, 18);
                    SerializedProperty serializedValue = serializedValues.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(serializedPosition, serializedValue, GUIContent.none);
                    i++;
                }
            }

            position = new Rect(25, position.y + 20, position.width - 5, 18);
            if (GUI.Button(position, "check for enum update") && value != null)
            {
                value.UpdateEnumList();
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var value = fieldInfo.GetValue(property.serializedObject.targetObject) as DataMap;
        if (!foldout || value == null)
        {
            return 18;
        }

        return (value.GetNumLabels() + 2) * 18 + 2;
    }
}
#endif