using System.Collections;
using UnityEngine;

public class Vertex
{
    public float height;

    public Vertex(float height)
    {
        SetHeight(height);
    }

    public void SetHeight(float height) {
        this.height = height;
    }
}