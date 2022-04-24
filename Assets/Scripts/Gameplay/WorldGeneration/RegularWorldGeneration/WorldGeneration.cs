using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen
{
    public class WorldGeneration : MonoBehaviour
    {
        [System.Serializable]
        public struct GenerationOptions
        {
            public bool sinIntensity;
            public WorldGen.SinWaveIntensity sinWaveIntensity;
            public bool diamondSquare;
            //public bool zeroDiamondSquareEdges;
            public float diamondSquareRoughness;
            public bool blurring;
            public int blurringIntensity;
            public bool erode;
            public WorldGen.ErosionData erosion;
        }

        public enum Generation
        {
            CreateHeightmap,
            SinIntensity,
            DiamondSquare,
            BlurringTerrain,
            Erode,
            CalculateNormals,
            GenerateWorldTiles,
            None,
            AllAtOnce,
            AITesting,
        }

        WorldTile[,] worldTiles;
        public WorldEditable worldEditable;
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
                case Generation.SinIntensity: SinWaveIntensity(); break;
                case Generation.DiamondSquare: DiamondSquare(); break;
                case Generation.BlurringTerrain: BlurTerrrain(); break;
                case Generation.Erode: Erode(); break;
                case Generation.CalculateNormals: CalculateNormals(); break;
                case Generation.GenerateWorldTiles: GenerateWorldTiles(); break;
            }

            if (currentState != Generation.None && currentState != Generation.AllAtOnce && currentState != Generation.AITesting)
            {
                currentState++;
            }
        }

        void InitializeHeightmap()
        {
            Utilities.Time(() =>
            {
                worldEditable = new WorldEditable
                    (worldData.tiles
                    , worldData.tiles
                    , worldData.tileSize
                    , worldData.tileScale
                    );
            }, currentState);
        }

        void GenerateWorldTiles()
        {
            Utilities.Time(() =>
            {
                worldTiles = worldEditable.GetWorldTiles(transform, worldTile, material);
            }, currentState);
        }

        void CalculateNormals()
        {
            Utilities.Time(() =>
            {
                worldEditable.CalculateNormals();
            }, currentState);
        }

        void DiamondSquare()
        {
            if (!generationData.diamondSquare) return;
            Utilities.Time(() =>
            {
                WorldGen.Generation.DiamondSquare(worldEditable, generationData.diamondSquareRoughness);
            }, currentState);
        }

        void BlurTerrrain()
        {
            if (!generationData.blurring) return;
            Utilities.Time(() =>
            {
                WorldGen.Generation.Blurring(worldEditable, generationData.blurringIntensity);
            }, currentState);
        }

        void AITesting()
        {
            InitializeHeightmap();
            WorldGen.Generation.AITesting(worldEditable);
            CalculateNormals();
            GenerateWorldTiles();
        }

        void Erode()
        {
            if (!generationData.erode) return;
            Utilities.Time(() =>
            {
                WorldGen.Generation.Erosion(worldEditable, generationData.erosion);
            }, currentState);
        }

        void SinWaveIntensity()
        {
            if (!generationData.sinIntensity) return;
            Utilities.Time(() =>
            {
                WorldGen.Generation.SinWaveIntensity(worldEditable, generationData.sinWaveIntensity);
            }, currentState);
        }
    }
}
