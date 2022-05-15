using System.Collections;
using UnityEngine;

public class Vertex
{
    public float height; // ( Y: Coluna, X: Linha (Y,X))
    public Color32 colour;

    public Vertex(float height)
    {
        SetHeight(height);
    }
    public Vertex(Color32 colour)
    {
        SetColour(colour);
    }

    public void SetHeight(float height) {
        this.height = height;
    }

    public void SetColour(Color32 colour)
    {
        this.colour = colour;
    }
}