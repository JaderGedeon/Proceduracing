using System.Collections;
using UnityEngine;

public class Vertex
{
    public float height; // ( Y: Coluna, X: Linha (Y,X))

    public Vertex(float height)
    {
        SetHeight(height);
    }

    public void SetHeight(float height) {
        this.height = height;
    }
}