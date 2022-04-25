using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace WorldGen
{
    public class FloatingWorldEditable
    {
        public bool flatSurfaceShading = true;
        public float[,] topHeightValues;
        public float[,] bottomHeightValues;
        public bool[,] isLandValues;

        public int xTiles;
        public int yTiles;
        public int tileSize;
        float scale;

        public FloatingWorldEditable(int xTiles, int yTiles, int tileSize, float tileScale)
        {
            this.xTiles = xTiles;
            this.yTiles = yTiles;
            this.tileSize = tileSize;
            this.scale = tileScale;

            this.topHeightValues = new float[xTiles * tileSize + 1, yTiles * tileSize + 1];
            this.bottomHeightValues = new float[xTiles * tileSize + 1, yTiles * tileSize + 1];
            this.isLandValues = new bool[xTiles * tileSize + 1, yTiles * tileSize + 1];
        }

        public void TranslateTop(float amount)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    topHeightValues[x, y] += amount;
                }
            }
        }

        public void TranslateTopByPercent(float amount, int iterations, int edgeThickness)
        {
            float maxValue = -1;
            float minValue = float.MaxValue;
            
            int w = bottomHeightValues.GetLength(0);
            int h = bottomHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (bottomHeightValues[x, y] < minValue)
                    {
                        minValue = bottomHeightValues[x, y];
                    }
                }
            }

            for (int i = 0; i < iterations; i++)
            {
                float percentageOfLand = 0;
                for (int x = 1; x < w - 1; x++)
                {
                    for (int y = 1; y < h - 1; y++)
                    {
                        percentageOfLand += topHeightValues[x, y] - edgeThickness > bottomHeightValues[x, y] ? 1 : 0;
                    }
                }

                percentageOfLand /= (w - 2) * (h - 2);
                float amountOffset = (amount / 100 - percentageOfLand) * (maxValue - minValue) / 2;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        topHeightValues[x, y] += amountOffset;
                    }
                }
            }
        }

        public void TranslateBottom(float amount)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    topHeightValues[x, y] += amount;
                }
            }
        }

        public void SetBooleanInterceptValues(int edgeThickness)
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    isLandValues[x, y] = true;
                    isLandValues[x, y] = topHeightValues[x, y] - edgeThickness > bottomHeightValues[x, y];
                }
            }
        }

        public void RemovePoorNeighbours()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    int check = 0;
                    if (!isLandValues[x, y])
                    {
                        continue;
                    }

                    if (isLandValues[x + 1, y]) check++;
                    if (isLandValues[x + 1, y + 1]) check++;
                    if (isLandValues[x + 1, y - 1]) check++;
                    if (isLandValues[x - 1, y]) check++;
                    if (isLandValues[x - 1, y + 1]) check++;
                    if (isLandValues[x - 1, y - 1]) check++;
                    if (isLandValues[x, y + 1]) check++;
                    if (isLandValues[x, y - 1]) check++;

                    isLandValues[x, y] = check > 3;
                }
            }
        }

        public void MirrorOffsetTerrain()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);

            float sumOfPoints = 0;
            for (int x = 1; x < w; x++)
            {
                for (int y = 1; y < h; y++)
                {
                    sumOfPoints += topHeightValues[x, y] > 0 ? 1 : -1;
                }
            }

            if (sumOfPoints < 0)
            {
                for (int x = 1; x < w; x++)
                {
                    for (int y = 1; y < h; y++)
                    {
                        topHeightValues[x, y] *= -1;
                    }
                }
            }

            sumOfPoints = 0;
            for (int x = 1; x < w; x++)
            {
                for (int y = 1; y < h; y++)
                {
                    sumOfPoints += bottomHeightValues[x, y] > 0 ? 1 : -1;
                }
            }

            if (sumOfPoints > 0)
            {
                for (int x = 1; x < w; x++)
                {
                    for (int y = 1; y < h; y++)
                    {
                        bottomHeightValues[x, y] *= -1;
                    }
                }
            }
        }

        public void MergeEdges()
        {
            int w = topHeightValues.GetLength(0);
            int h = topHeightValues.GetLength(1);
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (!isLandValues[x, y])
                    {
                        continue;
                    }

                    LODHeightEdgeAdjust(x, y, 1, out topHeightValues[x, y], out bottomHeightValues[x, y]);
                }
            }
        }

        public (WorldTile, WorldTile)[,] GetWorldTiles(Transform topParent, Transform bottomParent, WorldTile worldTilePrefab, Material sharedMaterial)
        {
            (WorldTile, WorldTile)[,] worldTiles = new (WorldTile, WorldTile)[xTiles, yTiles];
            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    WorldTile topWorldTile = GameObject.Instantiate<WorldTile>(worldTilePrefab);
                    topWorldTile.name = $"top tile {x}, {y}";
                    topWorldTile.transform.SetParent(topParent);
                    topWorldTile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);

                    WorldTile bottomWorldTile = GameObject.Instantiate<WorldTile>(worldTilePrefab);
                    bottomWorldTile.transform.SetParent(bottomParent);
                    bottomWorldTile.name = $"bottom tile {x}, {y}";
                    bottomWorldTile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);

                    SetTileData(ref topWorldTile, ref bottomWorldTile, sharedMaterial, x * tileSize, y * tileSize);

                    worldTiles[x, y] = (topWorldTile, bottomWorldTile);
                }
            }

            return worldTiles;
        }

        void SetTileData(ref WorldTile topTile, ref WorldTile bottomTile, Material sharedMaterial, int posX, int posY)
        {
            Mesh[] topLODMeshes = new Mesh[5];
            Mesh[] bottomLODMeshes = new Mesh[5];
            for (int i = 0; i < 5; i++)
            {
                var skip = 1 << i;
                topLODMeshes[i] = new Mesh();
                bottomLODMeshes[i] = new Mesh();
                if (!flatSurfaceShading)
                {
                    int[,] vertexIndices = new int[1 + tileSize, 1 + tileSize];

                    List<Vector2Int> vertices = new List<Vector2Int>();
                    int currentIndex = 0;
                    for (int x = 0; x < tileSize + 1; x += skip)
                    {
                        for (int y = 0; y < tileSize + 1; y += skip)
                        {
                            int dx = posX + x;
                            int dy = posY + y;

                            if (isLandValues[posX + x, posY + y])
                            {
                                vertices.Add(new Vector2Int(dx, dy));
                                vertexIndices[x, y] = currentIndex;
                                currentIndex++;
                            }
                        }
                    }

                    List<int> topTris = new List<int>();
                    List<int> bottomTris = new List<int>();
                    for (int x = 0; x < tileSize; x += skip)
                    {
                        for (int y = 0; y < tileSize; y += skip)
                        {
                            int dx = posX + x;
                            int dy = posY + y;
                            if (!isLandValues[dx, dy])
                            {
                                continue;
                            }

                            if (isLandValues[dx, dy] && isLandValues[dx + skip, dy] && isLandValues[dx + skip, dy + skip])
                            {
                                topTris.Add(vertexIndices[x, y]);
                                topTris.Add(vertexIndices[x + skip, y + skip]);
                                topTris.Add(vertexIndices[x + skip, y]);

                                bottomTris.Add(vertexIndices[x, y]);
                                bottomTris.Add(vertexIndices[x + skip, y]);
                                bottomTris.Add(vertexIndices[x + skip, y + skip]);
                            }

                            if (isLandValues[dx, dy] && isLandValues[dx + skip, dy + skip] && isLandValues[dx, dy + skip])
                            {
                                topTris.Add(vertexIndices[x, y]);
                                topTris.Add(vertexIndices[x, y + skip]);
                                topTris.Add(vertexIndices[x + skip, y + skip]);

                                bottomTris.Add(vertexIndices[x, y]);
                                bottomTris.Add(vertexIndices[x + skip, y + skip]);
                                bottomTris.Add(vertexIndices[x, y + skip]);
                            }
                        }
                    }

                    topLODMeshes[i].vertices = vertices
                        .Select(x =>
                        {
                            float topVal;
                            float throwAway;
                            LODHeightEdgeAdjust(x.x, x.y, skip, out topVal, out throwAway);
                            return new Vector3(x.x, topVal, x.y);
                        })
                        .ToArray();
                    topLODMeshes[i].triangles = topTris.ToArray();

                    bottomLODMeshes[i].vertices = vertices
                        .Select(x =>
                        {
                            float bottomVal;
                            float throwAway;
                            LODHeightEdgeAdjust(x.x, x.y, skip, out throwAway, out bottomVal);
                            return new Vector3(x.x, bottomVal, x.y);
                        })
                        .ToArray();
                    bottomLODMeshes[i].triangles = bottomTris.ToArray();

                    topLODMeshes[i].RecalculateBounds();
                    topLODMeshes[i].RecalculateNormals();
                    topLODMeshes[i].RecalculateTangents();

                    bottomLODMeshes[i].RecalculateBounds();
                    bottomLODMeshes[i].RecalculateNormals();
                    bottomLODMeshes[i].RecalculateTangents();
                }
                else
                {
                    List<Vector3> topVertices = new List<Vector3>();
                    List<Vector3> bottomVertices = new List<Vector3>();
                    List<int> tris = new List<int>();

                    int index = 0;
                    for (int x = 0; x < tileSize; x += skip)
                    {
                        for (int y = 0; y < tileSize; y += skip)
                        {
                            int dx = posX + x;
                            int dy = posY + y;
                            if (!isLandValues[dx, dy])
                            {
                                continue;
                            }

                            if (isLandValues[dx + skip, dy] && isLandValues[dx + skip, dy + skip])
                            {
                                topVertices.Add(new Vector3(x, topHeightValues[dx, dy], y));
                                topVertices.Add(new Vector3(x + skip, topHeightValues[dx + skip, dy + skip], y + skip));
                                topVertices.Add(new Vector3(x + skip, topHeightValues[dx + skip, dy], y));

                                bottomVertices.Add(new Vector3(x, bottomHeightValues[dx, dy], y));
                                bottomVertices.Add(new Vector3(x + skip, bottomHeightValues[dx + skip, dy], y));
                                bottomVertices.Add(new Vector3(x + skip, bottomHeightValues[dx + skip, dy + skip], y + skip));

                                tris.Add(index);
                                tris.Add(index + 1);
                                tris.Add(index + 2);
                                index += 3;
                            }

                            if (isLandValues[dx + skip, dy + skip] && isLandValues[dx, dy + skip])
                            {
                                topVertices.Add(new Vector3(x, topHeightValues[dx, dy], y));
                                topVertices.Add(new Vector3(x, topHeightValues[dx, dy + skip], y + skip));
                                topVertices.Add(new Vector3(x + skip, topHeightValues[dx + skip, dy + skip], y + skip));

                                bottomVertices.Add(new Vector3(x, bottomHeightValues[dx, dy], y));
                                bottomVertices.Add(new Vector3(x + skip, bottomHeightValues[dx + skip, dy + skip], y + skip));
                                bottomVertices.Add(new Vector3(x, bottomHeightValues[dx, dy + skip], y + skip));

                                tris.Add(index);
                                tris.Add(index + 1);
                                tris.Add(index + 2);
                                index += 3;
                            }
                        }
                    }

                    topLODMeshes[i].vertices = topVertices.ToArray();
                    topLODMeshes[i].triangles = tris.ToArray();
                    topLODMeshes[i].RecalculateBounds();
                    topLODMeshes[i].RecalculateNormals();

                    bottomLODMeshes[i].vertices = bottomVertices.ToArray();
                    bottomLODMeshes[i].triangles = tris.ToArray();
                    bottomLODMeshes[i].RecalculateBounds();
                    bottomLODMeshes[i].RecalculateNormals();
                }

                GameObject topLOD = new GameObject($"Top LOD {i}");
                topLOD.transform.position = new Vector3(posX, 0, posY);

                var topMeshRenderer = topLOD.AddComponent<MeshRenderer>();
                topMeshRenderer.sharedMaterial = sharedMaterial;
                var topMeshFilter = topLOD.AddComponent<MeshFilter>();
                topMeshFilter.mesh = topLODMeshes[i];
                topLOD.gameObject.isStatic = true;


                GameObject bottomLOD = new GameObject($"Bottom LOD {i}");
                bottomLOD.transform.position = new Vector3(posX, 0, posY);

                var BottomMeshRenderer = bottomLOD.AddComponent<MeshRenderer>();
                BottomMeshRenderer.sharedMaterial = sharedMaterial;
                var bottomMeshFilter = bottomLOD.AddComponent<MeshFilter>();
                bottomMeshFilter.mesh = bottomLODMeshes[i];
                bottomLOD.gameObject.isStatic = true;

                topTile.ApplyLOD(i, topLOD);
                bottomTile.ApplyLOD(i, bottomLOD);
            }
        }

        public void LODVectorEdgeAdjust(int x, int y, int step, out Vector3 top, out Vector3 bottom)
        {
            float topVal;
            float bottomVal;
            LODHeightEdgeAdjust(x, y, step, out topVal, out bottomVal);
            top = new Vector3(x, topVal, y);
            bottom = new Vector3(x, bottomVal, y);
        }

        public void LODHeightEdgeAdjust(int x, int y, int step, out float top, out float bottom)
        {
            if (isLandValues[x, y] && (
                !isLandValues[x + step, y] ||
                !isLandValues[x + step, y + step] ||
                !isLandValues[x + step, y - step] ||
                !isLandValues[x - step, y] ||
                !isLandValues[x - step, y + step] ||
                !isLandValues[x - step, y - step] ||
                !isLandValues[x, y + step] ||
                !isLandValues[x, y - step]))
            {
                top = (topHeightValues[x, y] + bottomHeightValues[x, y]) / 2;
                bottom = top;
                return;
            }

            top = topHeightValues[x, y];
            bottom = bottomHeightValues[x, y];
        }
    }
}