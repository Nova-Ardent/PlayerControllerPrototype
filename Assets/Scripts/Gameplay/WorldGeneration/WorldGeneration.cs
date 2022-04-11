using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class WorldGeneration : MonoBehaviour
    {
        [System.Serializable]
        public struct WorldData
        {
            public int tiles;

            public int tileSize;
            public float tileScale;

            public bool LODActive;
            public bool verbos;
            public string seed;
            public bool AITestingWorld;
        }

        [System.Serializable]
        public struct GenerationOptions
        {
            public bool diamondSquare;
            public float diamondSquareRoughness;
            public bool blurring;
            public int blurringIntensity;
        }

        public enum Generation
        {
            CreateHeightmap,
            DiamondSquare,
            BlurringTerrain,
            CalculateNormals,
            GenerateWorldTiles,
            None,
            AllAtOnce,
            AITesting,
        }

        WorldTile[,] worldTiles;
        WorldEditable worldEditable;
        Generation currentState;

        [SerializeField] WorldData worldData;
        [SerializeField] GenerationOptions generationData;
        [SerializeField] WorldTile worldTile;
        [SerializeField] Material material;

        // Start is called before the first frame update
        void Start()
        {
            WorldGen.Generation.SetSeed(worldData.seed);

            if (worldData.AITestingWorld)
            {
                currentState = Generation.AITesting;
            }
            else
            {
                currentState = Generation.CreateHeightmap;
            }
        }

        void Update()
        {
            StateUpdate();
        }

        void StateUpdate()
        {
            switch (currentState)
            {
                case Generation.None: break;
                case Generation.AllAtOnce:
                    currentState = Generation.None;
                    break;
                case Generation.AITesting:
                    AITesting();
                    currentState = Generation.None;
                    break;
                case Generation.CreateHeightmap: InitializeHeightmap(); break;
                case Generation.DiamondSquare: DiamondSquare(); break;
                case Generation.CalculateNormals: CalculateNormals(); break;
                case Generation.GenerateWorldTiles: GenerateWorldTiles(); break;
            }

            if (currentState != Generation.None && currentState != Generation.AllAtOnce && currentState != Generation.AITesting)
            {
                currentState++;
            }
        }

        void Print(bool isRunning, object data)
        {
            if (worldData.verbos)
            {
                Debug.Log($"{(isRunning ? "running" : "skipping")}: {data}");
            }
        }

        void InitializeHeightmap()
        {
            Print(true, currentState);
            worldEditable = new WorldEditable
                (worldData.tiles
                , worldData.tiles
                , worldData.tileSize
                , worldData.tileScale
                );
        }

        void GenerateWorldTiles()
        {
            Print(true, currentState);
            worldTiles = worldEditable.GetWorldTiles(transform, worldTile, material);
        }

        void CalculateNormals()
        {
            Print(true, currentState);
            worldEditable.CalculateNormals();
        }

        void DiamondSquare()
        {
            Print(generationData.diamondSquare, currentState);
            WorldGen.Generation.DiamondSquare(worldEditable, generationData.diamondSquareRoughness);
        }

        void BlurTerrrain()
        {

        }

        void AITesting()
        {
            InitializeHeightmap();
            WorldGen.Generation.AITesting(worldEditable);
            CalculateNormals();
            GenerateWorldTiles();
        }
    }

}
