using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public enum Rotation
    {
        Vertical, Horizontal, Diagonal
    }
    public Rotation rotation;
    public Vector2 V1 { private set; get; }
    public Vector2 V2 { private set; get; }
    public float[] Line { private set; get; }
    public Edge(Vector2 v1, Vector2 v2)
    {
        if (v1.x == v2.x)
        {
            rotation = Rotation.Vertical;
        }
        else if (v1.y == v2.y)
        {
            rotation = Rotation.Horizontal;
        }
        else
        {
            rotation = Rotation.Diagonal;
        }
        V1 = v1;
        V2 = v2;
        Line = LinAl.GetLine(v1, v2);
    }

}
