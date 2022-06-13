using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
public static class MarchingCubeUVLookupGenerator
{
    static string fileNameCS = "./Assets/Scripts/Gameplay/WorldGeneration/MarchingCubeGenerator/MarchingCubeTexturing/MarchingCubeUVLookup.cs";
    static string fileNameHLSL = "./Assets/Scripts/Gameplay/WorldGeneration/ComputeShaders/MarchingCubeUVLookup.compute";

    enum TerrainType
    {
        None,
        Grass,
        Dirt,
    }

    [UnityEditor.MenuItem("FileGenerator/Marching Cubes Lookup")]
    static void GenerateMarchingCubeUVs()
    {
        FileGenerator generator = new CSLookup();
        generator.GenerateFile(fileNameCS);

        generator = new HLSLLookup();
        generator.GenerateFile(fileNameHLSL);
    }

    class CSLookup : CSharpFileGenerator
    {
        protected override void GenerateFileText()
        {
            Line("using UnityEngine;");
            Line();

            using (NameSpace("WorldGen"))
            {
                using (Class("MarchingCubeUVLookup", Accessibility.Public))
                {
                    TerrainType[] terrainTypes = Utilities.GetEnums<TerrainType>().ToArray();
                    using (Enum("TerrainType", Accessibility.Public))
                    {
                        for (int i = 0; i < terrainTypes.Length; i++)
                        {
                            Line(terrainTypes[i].ToString() + ",");
                        }
                    }

                    Line();
                    Line("public static Vector2[,] terrainCombinations = new Vector2[,]");
                    using (Stack(';'))
                    {
                        for (int i = 0; i < terrainTypes.Length; i++)
                        {

                            using (Stack(','))
                            {
                                for (int j = 0; j < terrainTypes.Length; j++)
                                {
                                    var x = Mathf.Min(i, j);
                                    var y = Mathf.Max(i, j);
                                    if (x == y)
                                    {
                                        x = (int)TerrainType.None;
                                    }

                                    Line($"new Vector2({(x + 0.5f) / terrainTypes.Length}f, {(y + 0.5f) / terrainTypes.Length}f), // {terrainTypes[i]} + {terrainTypes[j]} | pixel ({x}, {y})");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class HLSLLookup : HLSLFileGenerator
    {
        protected override void GenerateFileText()
        {
            TerrainType[] terrainTypes = Utilities.GetEnums<TerrainType>().ToArray();

            Line($"static const int MarchingCubeUVLookup[{terrainTypes.Length}][{terrainTypes.Length}] =");
            using (Stack(';'))
            {
                for (int i = 0; i < terrainTypes.Length; i++)
                {

                    using (Stack(','))
                    {
                        for (int j = 0; j < terrainTypes.Length; j++)
                        {
                            var x = Mathf.Min(i, j);
                            var y = Mathf.Max(i, j);
                            if (x == y)
                            {
                                x = (int)TerrainType.None;
                            }

                            Line($"float2({(x + 0.5f) / terrainTypes.Length}f, {(y + 0.5f) / terrainTypes.Length}f), // {terrainTypes[i]} + {terrainTypes[j]} | pixel ({x}, {y})");
                        }
                    }
                }
            }
        }
    }
}
#endif