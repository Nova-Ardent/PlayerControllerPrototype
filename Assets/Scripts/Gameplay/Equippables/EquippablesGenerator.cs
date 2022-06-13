using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
public static class EquippablesGenerator
{
    static string fileName = "./Assets/Scripts/Gameplay/Equippables/Equippables.cs";

    static string CategoryPath = "./Assets/GameObjects/Characters/Humans/PlayerCharacters/Equippables";


    [UnityEditor.MenuItem("FileGenerator/Equippables")]
    static void GenerateEquippablesList()
    {
        FileGenerator generator = new Equippables();
        generator.GenerateFile(fileName);
    }

    class Equippables : CSharpFileGenerator
    {
        protected override void GenerateFileText()
        {
            Line("using System;");
            Line("using System.Linq;");
            Line("using UnityEngine;");
            Line("using Newtonsoft.Json;");
            Line("using Newtonsoft.Json.Converters;");
            Line("using System.Collections.Generic;");

            Line();
            Line("[CreateAssetMenu(fileName = \"Equippables\", menuName = \"ScriptableObjects/Equippables\", order = 1)]");
            using (Class("Equippables", Accessibility.Public, extends : "ScriptableObject"))
            {
                string[] categoryFiles = Directory.GetDirectories(CategoryPath)
                    .Select(x => Path.GetFileNameWithoutExtension(x))
                    .ToArray();

                foreach (var category in categoryFiles)
                {
                    Line("[JsonConverter(typeof(StringEnumConverter))]");
                    using (Enum(category.ToString(), Accessibility.Public))
                    {
                        string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{category}")
                            .Select(x => Path.GetFileNameWithoutExtension(x))
                            .OrderByDescending(x => x[0] == '_')
                            .ToArray();

                        foreach (var itemFile in itemFiles)
                        {
                            Line(itemFile + ",");
                        }
                    }
                    Line("");
                }

                foreach (var category in categoryFiles)
                {
                    Line("[System.Serializable]");
                    using (Struct(category.ToString() + "Prefabs", Accessibility.Public))
                    {
                        string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{category}")
                            .Select(x => Path.GetFileNameWithoutExtension(x))
                            .OrderByDescending(x => x[0] == '_')
                            .ToArray();

                        foreach (var itemFile in itemFiles)
                        {
                            Line($"public GameObject {itemFile};");
                        }
                    }
                    Line();
                }

                foreach (var category in categoryFiles)
                {
                    Line($"public {category}Prefabs {FormateObjectName(category)};");
                }

                foreach (var category in categoryFiles)
                {
                    Line();
                    string[] itemFiles = Directory.GetDirectories(CategoryPath + $"/{category}")
                        .Select(x => Path.GetFileNameWithoutExtension(x))
                        .OrderByDescending(x => x[0] == '_')
                        .ToArray();

                    using (Function($"Get{category}", Accessibility.Public, "GameObject", $"{category} val"))
                    {
                        using (Switch("val"))
                        {
                            Line("default:");
                            foreach (var itemFile in itemFiles)
                            {
                                Line($"case {category}.{itemFile}: return {FormateObjectName(category)}.{itemFile};");
                            }
                        }
                    }
                }
            }
        }

        string FormateObjectName(string category)
        {
            return char.ToLower(category[0]) + category.Substring(1);
        }
    }
}
#endif