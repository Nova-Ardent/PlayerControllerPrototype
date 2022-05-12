using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace WorldGen
{
    public class WorldEditable
    {
        struct FaceNormalGroup
        {
            public Vector3 upRight;
            public Vector3 lowerLeft;
        }

        float[,] _heightMap;
        Vector3[,] normals;

        public float[,] heightMap
        {
            get => _heightMap;
            set => _heightMap = value;
        }

        public int xTiles;
        public int yTiles;
        public int tileSize;

        float scale;

        public WorldEditable(int xTiles, int yTiles, int tileSize, float tileScale)
        {
            this.xTiles = xTiles;
            this.yTiles = yTiles;
            this.tileSize = tileSize;
            this.scale = tileScale;

            heightMap = new float[xTiles * tileSize + 1, yTiles * tileSize + 1];
            normals = new Vector3[xTiles * tileSize + 1, yTiles * tileSize + 1];
        }

        public void CalculateNormals()
        {
            // test doing this with a compute shader

            int width = xTiles * tileSize;
            int height = yTiles * tileSize;
            FaceNormalGroup[,] faceNormalGroups = new FaceNormalGroup[heightMap.GetLength(0) - 1, heightMap.GetLength(1) - 1];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    faceNormalGroups[x, y].upRight = CalculateFace(x, y, x + 1, y, x + 1, y + 1);
                    faceNormalGroups[x, y].lowerLeft = CalculateFace(x, y, x + 1, y + 1, x, y + 1);
                }
            }

            for (int x = 0; x < normals.GetLength(0); x++)
            {
                for (int y = 0; y < normals.GetLength(1); y++)
                {
                    normals[x, y] = Vector3.zero;
                    if (x < width && y < height)
                    {
                        normals[x, y] += faceNormalGroups[x, y].upRight;
                        normals[x, y] += faceNormalGroups[x, y].lowerLeft;
                    }

                    if (x - 1 > 0 && y - 1 > 0)
                    {
                        normals[x, y] += faceNormalGroups[x - 1, y - 1].upRight;
                        normals[x, y] += faceNormalGroups[x - 1, y - 1].lowerLeft;
                    }

                    if (x < width && y - 1 > 0)
                    {
                        normals[x, y] += faceNormalGroups[x, y - 1].lowerLeft;
                    }

                    if (x - 1 > 0 && y < height)
                    {
                        normals[x, y] += faceNormalGroups[x - 1, y].upRight;
                    }

                    normals[x, y].Normalize();
                }
            }
        }

        Vector3 CalculateFace(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Vector3 v1 = new Vector3(x1 * scale, heightMap[x1, y1], y1 * scale);
            Vector3 v2 = new Vector3(x2 * scale, heightMap[x2, y2], y2 * scale);
            Vector3 v3 = new Vector3(x3 * scale, heightMap[x3, y3], y3 * scale);

            Vector3 V = v2 - v1;
            Vector3 U = v3 - v1;
            Vector3 N;

            N.x = U.y * V.z - U.z * V.y;
            N.y = U.z * V.x - U.x * V.z;
            N.z = U.x * V.y - U.y * V.x;
            return N;
        }

        public WorldTile[,] GetWorldTiles(Transform parent, WorldTile worldTilePrefab, Material sharedMaterial)
        {
            WorldTile[,] worldTiles = new WorldTile[xTiles, yTiles];
            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    worldTiles[x, y] = GameObject.Instantiate<WorldTile>(worldTilePrefab);
                    worldTiles[x, y].transform.SetParent(parent);

                    SetTileData(ref worldTiles[x, y], sharedMaterial, x * tileSize, y * tileSize);
                }
            }

            return worldTiles;
        }

        void SetTileData(ref WorldTile tile, Material sharedMaterial, int posX, int posY)
        {
            Mesh[] lodMeshes = new Mesh[5];

            for (int i = 0; i < 5; i++)
            {
                var skip = 1 << i;
                lodMeshes[i] = new Mesh();

                int vertex = 0;
                int tri = 0;

                int indices = (1 + tileSize / skip);
                Vector3[] vertices = new Vector3[indices * indices];
                Vector3[] normals = new Vector3[indices * indices];
                int[] tris = new int[6 * indices * indices];


                for (int x = 0; x < tileSize + 1; x += skip)
                {
                    for (int y = 0; y < tileSize + 1; y += skip)
                    {
                        int vertexX = posX + x;
                        int vertexY = posY + y;

                        try
                        {
                            vertices[vertex] = new Vector3(vertexX * scale, heightMap[vertexX, vertexY], vertexY * scale);
                            normals[vertex] = this.normals[vertexX, vertexY];
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }

                        if (x != tileSize && y != tileSize)
                        {
                            tris[tri] = vertex;
                            tris[tri + 1] = vertex + 1;
                            tris[tri + 2] = vertex + indices;
                            tris[tri + 3] = vertex + 1;
                            tris[tri + 4] = vertex + indices + 1;
                            tris[tri + 5] = vertex + indices;
                        }

                        vertex++;
                        tri += 6;
                    }
                }

                lodMeshes[i].vertices = vertices;
                lodMeshes[i].triangles = tris;
                lodMeshes[i].RecalculateBounds();
                lodMeshes[i].normals = normals;
                lodMeshes[i].RecalculateTangents();

                GameObject lod = new GameObject($"LOD {i}");



                var meshRenderer = lod.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = sharedMaterial;

                var meshFilter = lod.AddComponent<MeshFilter>();
                meshFilter.mesh = lodMeshes[i];

                lod.gameObject.isStatic = true;
                tile.ApplyLOD(i, lod);
            }
        }
    }
}
