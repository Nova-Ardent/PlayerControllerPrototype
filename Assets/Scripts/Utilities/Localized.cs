#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public partial class Localized
{
    private static Localized? _instance;
    public static Localized Instance
    {   get 
        {
            if (_instance == null)
            {
                _instance = new Localized();
            }
            return _instance;
        } 
    }

    public const string RESOURCE_LANGUAGES_PATH = "Localization/Languages";
    public const string LANGUAGES_PATH = "Assets/Resources/Localization/Languages";

    public int currentLanguage;
    public string[] languages =
        { "English_NA"
        , "Dutch"
        };

    SortedDictionary<string, string>? languageLibrary;

    public Localized()
    {
        
    }

    public void SetLanguage(int lang)
    {
        this.currentLanguage = lang;
        this.languageLibrary = LoadLanguage(languages[lang]);
    }

    public void ValidateAndCreateLanguages()
    {
#if UNITY_EDITOR
        foreach (var lang in languages)
        {
            string path = LANGUAGES_PATH + $"/{lang}" + ".txt";
            if (AssetDatabase.FindAssets($"{lang}").Length == 0)
            {
                string defaultText = JsonConvert.SerializeObject(GenerateEmptyDictionary(), Formatting.Indented);

                var sw = new System.IO.StreamWriter(path);
                sw.Write(defaultText);
                sw.Close();
            }
            else
            {
                bool dirty = false;
                string updatedText = JsonConvert.SerializeObject(LoadLanguage(lang, out dirty), Formatting.Indented);
                if (dirty)
                {
                    var usw = new System.IO.StreamWriter(path);
                    usw.Write(updatedText);
                    usw.Close();
                }
            }
        }
#endif
    }

    public SortedDictionary<string, string> LoadLanguage(string lang)
    {
        bool dirty;
        return LoadLanguage(lang, out dirty);
    }

    public SortedDictionary<string, string> LoadLanguage(string lang, out bool dirty)
    {
        string path = RESOURCE_LANGUAGES_PATH + $"/{lang}";
        TextAsset? textAsset = Resources.Load<TextAsset>(path);

        dirty = false;

        if (textAsset != null)
        {
            SortedDictionary<string, string> ret = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(textAsset.text);
            foreach (var enumType in typeof(Localized).GetNestedTypes())
            {
                if (!enumType.IsEnum)
                {
                    continue;
                }

                foreach (var enumVal in Utilities.GetEnums(enumType))
                {
                    if (!ret.ContainsKey(enumVal.ToString()))
                    {
                        dirty = true;
                        ret.Add(enumVal.ToString(), "");
                    }
                }
            }

            return ret;
        }
        return GenerateEmptyDictionary();
    }

    public SortedDictionary<string, string> GenerateEmptyDictionary()
    {
        SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
        foreach (var enumType in typeof(Localized).GetNestedTypes())
        {
            if (!enumType.IsEnum)
            {
                continue;
            }

            foreach (var enumVal in Utilities.GetEnums(enumType))
            {
                var key = enumVal.ToString();
                if (!keys.ContainsKey(key))
                {
                    keys.Add(key, "");
                }
            }
        }
        
        return keys;
    }

    public string GetDefinition(Enum val, params object[] values)
    {
        if (languageLibrary == null)
        {
            Debug.LogError("attempting to get text value without selecting a language first.");
            return val.ToString();
        }
        
        var ret = languageLibrary[val.ToString()];

        if (String.IsNullOrWhiteSpace(ret))
        {
            ret = val.GetType().BaseType.ToString() + "." + val.ToString();
        }
        return string.Format(ret, values);
    }
}

public static class LocalizedExtensions
{
    public static string Localize(this Enum val, params object[] values)
    {
        return Localized.Instance.GetDefinition(val, values);
    }
}
