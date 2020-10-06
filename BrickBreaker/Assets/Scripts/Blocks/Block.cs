using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
    protected float w, h;
    public Vector2 pos;
    public Vector2[] verts;
    public Edge[] edges;
    public virtual void SetVerticies(Vector2 pos, float rot)
    {

    }
    public virtual void SetEdges()
    {
        
    }
    
}
