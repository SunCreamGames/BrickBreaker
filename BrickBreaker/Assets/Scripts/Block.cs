using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    protected float w, h, rotation;
    protected Vector2 pos;
    public Vector2[] verts;
    public float[][] edges { private set; get; }
    public virtual Vector2[] SetVerticies()
    {
        return null;
    }
    public virtual float[][] SetEdges()
    {
        return null;
    }
    public virtual bool CheckForCollision(Vector2 pos)
    {
        return false;
    }
    public virtual void GetReflectData(out LinAl.Reflect reflect, out Vector2 pointOfReflect, Vector2 pos, Vector2 prevPos)
    {
        reflect = LinAl.Reflect.Diagonal;
        pointOfReflect = Vector2.zero;
    }
    public virtual void GetReflectData(out float[] line, out Vector2 pointOfReflect, Vector2 pos, Vector2 prevPos)
    {
        line = new float[3];
        pointOfReflect = Vector2.zero;
    }
}
