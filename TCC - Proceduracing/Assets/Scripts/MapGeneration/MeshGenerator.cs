using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    private Vector2Int size;

    Vector2 topLeft;

    public void Generate(Vector2Int mapSize, Vertex[,] noiseMap, float heightMultiplier) {
        size = mapSize;
        topLeft = new Vector2((mapSize.x - 1) / -2f, (mapSize.y - 1) / 2f);

        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices(noiseMap, heightMultiplier);
        mesh.triangles = CreateTriangles();
        mesh.uv = CreateUVs();

        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    // Distribui os pontos dos vértices ao longo de toda a malha, armazeando-os num só array de Vec3.
    private Vector3[] CreateVertices(Vertex[,] noiseMap, float heightMultiplier) {

        Vector3[] vertices = new Vector3[size.x * size.y];

        int i = 0;
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {

                vertices[i] = new Vector3(topLeft.x + x , noiseMap[x,z].height * heightMultiplier, topLeft.y - z);
                i++;
            }
        }
        return vertices;
    }

    /* Contrói os triângulos baseado num sentido horário da posição dos pontos, armazeando-os num só array de Vec3.

    1 ─ ─ ─ 3
    ¦       ¦
    ¦   X   ¦
    ¦       ¦
    0 ─ ─ ─ 2       

    */
    private int[] CreateTriangles() {

        int[] triangles = new int[(size.x - 1) * (size.y - 1) * 6];

        int vert = 0, tris = 0;

        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (x < size.x - 1 && z < size.y - 1) {

                    triangles[tris] = vert;
                    triangles[tris + 1] = vert + size.x + 1;
                    triangles[tris + 2] = vert + size.x;
                    triangles[tris + 3] = vert + size.x + 1;
                    triangles[tris + 4] = vert;
                    triangles[tris + 5] = vert + 1;

                    vert++;
                    tris += 6;

                }
            }
            vert++;
        }

        return triangles;
    }

    private Vector2[] CreateUVs() {

        Vector2[] uvs = new Vector2[size.x * size.y];

        int i = 0;
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                uvs[i] = new Vector2(x / (float)size.x, z / (float)size.y);
                i++;
            }
        }

        return uvs;
    }
}
