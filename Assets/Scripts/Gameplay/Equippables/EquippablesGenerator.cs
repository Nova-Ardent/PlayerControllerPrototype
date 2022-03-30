using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class EquippablesGenerator
{
    static string fileName = "./Assets/Scripts/Gameplay/Equippables/Equippables.cs";

    static string body =
@"// Do not manually modify this file. This file has been procedurally generated.

using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

[CreateAssetMenu(fileName = ""Equippables"", menuName = ""ScriptableObjects/Equippables"", order = 1)]
public class Equippables : ScriptableObject
{{
{0}
}}
";

    static string enums =
@"    [JsonConverter(typeof(StringEnumConverter))]
    public enum {0}
    {{
{1}    }}

";

    static string serializableStruct =
@"    [System.Serializable]
    public struct {0}Prefabs
    {{
{1}    }}

";

    static string getGOFunction =
@"    public GameObject Get{0}({0} val)
    {{
        switch (val)
        {{
            default:
{1}        }}
    }}

";


    static string structVal = "    public {0}Prefabs {1};\n";
    static string enumVal = "        {0},\n";
    static string GOVals = "        public GameObject {0};\n";
    static string cases = "            case {1}.{0}: return {2}.{0};\n";

    static string CategoryPath = "./Assets/GameObjects/Characters/Humans/PlayerCharacters/Equippables";

#if UNITY_EDITOR
    [UnityEditor.MenuItem("FileGenerator/Equippables")]
#endif
    static void GenerateElementsList()
    {
        var sw = new StreamWriter(fileName);
        sw.Write(GenerateBody());
        sw.Close();
    }

    static string GenerateBody()
    {
        return string.Format(body, BodyInformation());
    }

    static string BodyInformation()
    {
        string[] categoryFiles = Directory.GetDirectories(CategoryPath)
            .Select(x => Path.GetFileNameWithoutExtension(x))
            .ToArray();

        string structs = "";
        foreach (var category in categoryFiles)
        {
            structs += string.Format(enums, category, EnumNames(category));
        }

        foreach (var category in categoryFiles)
        {
            structs += string.Format(serializableStruct, category, GONames(category));
        }

        foreach (var category in categoryFiles)
        {
            structs += string.Format(structVal, category, char.ToLower(category[0]) + category.Substring(1));
        }
        structs += "\n";

        foreach (var category in categoryFiles)
        {
            structs += string.Format(getGOFunction, category, CaseNames(category));
        }

        return structs;
    }

    static string EnumNames(string path)
    {
        string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{path}")
            .Select(x => Path.GetFileNameWithoutExtension(x))
            .OrderByDescending(x => x[0] == '_')
            .ToArray();

        string values = "";
        foreach(var item in itemFiles)
        {
            values += string.Format(enumVal, item);
        }

        return values;
    }

    static string GONames(string path)
    {
        string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{path}")
            .Select(x => Path.GetFileNameWithoutExtension(x))
            .OrderByDescending(x => x[0] == '_')
            .ToArray();

        string values = "";
        foreach (var item in itemFiles)
        {
            values += string.Format(GOVals, item);
        }

        return values;
    }

    static string CaseNames(string path)
    {
        string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{path}")
            .Select(x => Path.GetFileNameWithoutExtension(x))
            .OrderByDescending(x => x[0] == '_')
            .ToArray();

        string values = "";
        foreach (var item in itemFiles)
        {
            values += string.Format(cases, item, path, char.ToLower(path[0]) + path.Substring(1));
        }

        return values;
    }

}