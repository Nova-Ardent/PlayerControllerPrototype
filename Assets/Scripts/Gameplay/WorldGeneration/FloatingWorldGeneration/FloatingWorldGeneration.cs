using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace WorldGen
{
    public class FloatingWorldGeneration : MonoBehaviour
    {
        public enum TopTranslation
        {
            Off,
            Constant,
            PercentageAccessibleLand,
        }

        public enum State
        {
            None,
            InitializeHeightMap,
            DiamondSquare,
            MirrorOffsetting,
            TranslateTopTerrain,
            BooleanIntercection,
            RemoveNeighbourlessPoints,
            MergeEdges,
            AllPointsAboveZero,
            GenerateMeshes,
            Done,
        }


        [System.Serializable]
        public class GenerationData
        {
            public bool diamondSquare;
            public float diamondSquareRoughness;
        }

        [SerializeField] WorldData worldData;

        [Header("generation data")]
        public string islandName = "";
        public int edgeThickness;
        public int edgecleaningIterations;
        public TopTranslation topTranslation;
        public float topTranslationValue;
        public int topTranslationIterations;
        public bool disableLODs;

        [SerializeField] GenerationData topGenerationData;
        [SerializeField] GenerationData bottomGenerationData;
        [SerializeField] WorldTile worldTile;
        [SerializeField] Material material;
        [SerializeField] Camera camera;

        State state;
        FloatingWorldEditable floatingWorldEditable;
        [SerializeField] MarchingCubeChunkGenerator marchingCubeChunkLoader;

        // Start is called before the first frame update
        void Start()
        {
            WorldGen.Generation.SetSeed(worldData.seed);
            state = State.InitializeHeightMap;
            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        void Update()
        {
            StateUpdate();
        }

        void StateUpdate()
        {
            switch (state)
            {
                case State.Done:
                    Destroy(this);
                    break;
                case State.None: break;
                case State.InitializeHeightMap: InitializeHeightmap(); break;
                case State.DiamondSquare: DiamondSquare(); break;
                case State.MirrorOffsetting: MirrorOffsetTerrain(); break;
                case State.TranslateTopTerrain: TranslateTopTerrain(); break;
                case State.BooleanIntercection: BooleanIntercept(); break;
                case State.RemoveNeighbourlessPoints: RemovePoorNeighbours(); break;
                case State.MergeEdges: MergeEdges(); break;
                case State.AllPointsAboveZero: AllPointsAboveZero(); break;
                case State.GenerateMeshes: GenerateMeshes(); break;
            }

            if (state != State.None && state != State.Done)
            {
                state++;
            }
        }

        void InitializeHeightmap()
        {
            Utilities.Time(() =>
            {
                floatingWorldEditable = new FloatingWorldEditable
                    (worldData.tiles
                    , worldData.tiles
                    , worldData.tileSize
                    , worldData.tileScale
                    );
            }, state);
        }

        void DiamondSquare()
        {
            if (!topGenerationData.diamondSquare) return;

            Utilities.Time(() =>
            {
                WorldGen.Generation.DiamondSquare
                ( floatingWorldEditable
                , topGenerationData.diamondSquare
                , topGenerationData.diamondSquareRoughness
                , bottomGenerationData.diamondSquare
                , bottomGenerationData.diamondSquareRoughness
                );
            }, state);
        }

        void TranslateTopTerrain()
        {
            if (topTranslation == TopTranslation.Off) return;

            if (topTranslation == TopTranslation.Constant)
            {
                Utilities.Time(() =>
                {
                    floatingWorldEditable.TranslateTop(topTranslationValue);
                }, state);
            }            
            else if (topTranslation == TopTranslation.PercentageAccessibleLand)
            {
                Utilities.Time(() =>
                {
                    floatingWorldEditable.TranslateTopByPercent(topTranslationValue, topTranslationIterations, edgeThickness);
                }, state);
            }
        }

        void MirrorOffsetTerrain()
        {
            Utilities.Time(() =>
            {
                floatingWorldEditable.MirrorOffsetTerrain();
            }, state);
        }

        void BooleanIntercept()
        {
            Utilities.Time(() =>
            {
                floatingWorldEditable.SetBooleanInterceptValues(edgeThickness);
            }, state);
        }

        void RemovePoorNeighbours()
        {
            Utilities.Time(() =>
            {
                for (int i = 0; i < edgecleaningIterations; i++)
                {
                    floatingWorldEditable.RemovePoorNeighbours();
                }
            }, state);
        }

        void MergeEdges()
        {
            Utilities.Time(() =>
            {
                floatingWorldEditable.MergeEdges();
            }, state);
        }

        void AllPointsAboveZero()
        {
            Utilities.Time(() =>
            {
                floatingWorldEditable.TranslateAboveZero();
            }, state);
        }

        void GenerateMeshes()
        {
            Utilities.Time(() =>
            {
                marchingCubeChunkLoader.LoadPointsFromFloatingWorld(floatingWorldEditable, worldData.seed);
            }, $"{state} p1");

            Utilities.Time(() =>
            {
                marchingCubeChunkLoader.GenerateAllChunks();
            }, $"{state} p2");
        }
    }
}
