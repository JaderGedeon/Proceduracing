using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public static Mesh GenerateTerrainMesh(Vertex[,] noiseMap, float heightMultiplier, bool flatShading)
    {
        Vector2Int meshSize = new Vector2Int(noiseMap.GetLength(0), noiseMap.GetLength(1));

        MeshData meshData = new MeshData(meshSize, flatShading);
        int vertexIndex = 0;

        for (int y = 0; y < meshSize.y; y++)
        {
            for (int x = 0; x < meshSize.x; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(x, noiseMap[x, y].height * heightMultiplier, y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)meshSize.x, y / (float)meshSize.y);

                if (x < meshSize.x - 1 && y < meshSize.y - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + meshSize.x, vertexIndex + meshSize.x + 1);
                    meshData.AddTriangle(vertexIndex + meshSize.x + 1, vertexIndex + 1, vertexIndex);
                }
                vertexIndex++;
            }
        }
        return meshData.CreateMesh();
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;
    bool flatShading;

    public MeshData(Vector2Int meshSize, bool flatShading)
    {
        vertices = new Vector3[meshSize.x * meshSize.y];
        uvs = new Vector2[meshSize.x * meshSize.y];
        triangles = new int[(meshSize.x - 1) * (meshSize.y - 1) * 6];
        this.flatShading = flatShading;
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    Vector3[] CalculateNormals() 
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length;

        for (int i = 0; i < triangleCount - 3; i++)
        {
            int normalTriangleIndex = i;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = vertices[indexA];
        Vector3 pointB = vertices[indexB];
        Vector3 pointC = vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (flatShading)
        {
            FlatShading();
            mesh.RecalculateNormals();
        }
        else 
        {
            mesh.normals = CalculateNormals();
        }
        return mesh;
    }

    void FlatShading() {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }
}