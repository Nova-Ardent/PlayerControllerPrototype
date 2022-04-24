using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            TranslateTopTerrain,
            BooleanIntercection,
            RemoveNeighbourlessPoints,
            MergeEdges,
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
        public int edgeThickness;
        public int edgecleaningIterations;
        public TopTranslation topTranslation;
        public float topTranslationValue;
        public int topTranslationIterations;

        [SerializeField] GenerationData topGenerationData;
        [SerializeField] GenerationData bottomGenerationData;
        [SerializeField] WorldTile worldTile;
        [SerializeField] Material material;

        State state;
        FloatingWorldEditable floatingWorldEditable;

        // Start is called before the first frame update
        void Start()
        {
            WorldGen.Generation.SetSeed(worldData.seed);
            state = State.InitializeHeightMap;
        }

        void Update()
        {
            StateUpdate();
        }

        void StateUpdate()
        {
            switch (state)
            {
                case State.Done: break;
                case State.None: break;
                case State.InitializeHeightMap: InitializeHeightmap(); break;
                case State.DiamondSquare: DiamondSquare(); break;
                case State.TranslateTopTerrain: TranslateTopTerrain(); break;
                case State.BooleanIntercection: BooleanIntercept(); break;
                case State.RemoveNeighbourlessPoints: RemovePoorNeighbours(); break;
                case State.MergeEdges: MergeEdges(); break;
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

        void GenerateMeshes()
        {
            var top = new GameObject("top");
            var bottom = new GameObject("bottom");

            top.transform.SetParent(this.transform);
            bottom.transform.SetParent(this.transform);

            Utilities.Time(() =>
            {
                floatingWorldEditable.GetWorldTiles(top.transform, bottom.transform, worldTile, material);
            }, state);
        }
    }
}
