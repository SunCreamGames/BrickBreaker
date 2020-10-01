using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : Block
{
    override public Vector2[] SetVerticies()
    {
        verts = new Vector2[4];
        verts[0] = pos + new Vector2(-w / 2, h / 2);
        verts[1] = pos + new Vector2(w / 2, h / 2);
        verts[2] = pos + new Vector2(w / 2, -h / 2);
        verts[3] = pos + new Vector2(-w / 2, -h / 2);
        return verts;
    }
    public override float[][] SetEdges()
    {
        float[][] edges = new float[4][];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new float[5];
            float[] newEdge = LinAl.GetLine(verts[i], verts[(i + 1) % 4]);
            for (int k = 0; k < 3; k++)
            {
                edges[i][k] = newEdge[k];
            }
            edges[i][3] = Mathf.Min(verts[i].x, verts[(i + 1) % 4].x);
            edges[i][4] = Mathf.Max(verts[i].x, verts[(i + 1) % 4].x);
        }
        return edges;
    }
    override public void GetReflectData(out LinAl.Reflect reflect, out Vector2 pointOfReflect, Vector2 pos, Vector2 prevPos)
    {
        float[] velocityLine;
         velocityLine = LinAl.GetLine(pos, prevPos);
        if (prevPos.y >= verts[0].y)
        {
            if (prevPos.x <= verts[0].x)
            {
                reflect = LinAl.Reflect.Diagonal;
                pointOfReflect = verts[0];
            }
            else if (prevPos.x >= verts[1].x)
            {
                reflect = LinAl.Reflect.Diagonal;
                pointOfReflect = verts[1];
            }
            else
            {
                reflect = LinAl.Reflect.Horizontal;
                pointOfReflect = LinAl.CrossOfTheLines(
                    LinAl.GetLine(verts[0], verts[1]),
                    LinAl.GetLine(pos, prevPos));
            }
        }
        else if (prevPos.y <= verts[2].y)
        {
            if (prevPos.x <= verts[3].x)
            {
                reflect = LinAl.Reflect.Diagonal;
                pointOfReflect = verts[3];
            }
            else if (prevPos.x >= verts[2].x)
            {
                reflect = LinAl.Reflect.Diagonal;
                pointOfReflect = verts[2];
            }
            else
            {
                reflect = LinAl.Reflect.Horizontal;
                pointOfReflect = LinAl.CrossOfTheLines(
                    LinAl.GetLine(verts[2], verts[3]),
                    LinAl.GetLine(pos, prevPos));
            }
        }
        else
        {
            if (prevPos.x <= verts[0].x)
            {
                reflect = LinAl.Reflect.Vertical;
                pointOfReflect = LinAl.CrossOfTheLines(
                    LinAl.GetLine(verts[0], verts[3]),
                    LinAl.GetLine(pos, prevPos));
            }
            else
            {
                reflect = LinAl.Reflect.Vertical;
                pointOfReflect = LinAl.CrossOfTheLines(
                    LinAl.GetLine(verts[2], verts[1]),
                    LinAl.GetLine(pos, prevPos));
            }
        }
    }
    override public bool CheckForCollision(Vector2 pos)
    {
        if (pos.x < verts[1].x && pos.y < verts[1].y && pos.x > verts[3].x && pos.y > verts[3].y)
        {
            return true;
        }
        return false;
    }
    public NormalBlock(float w, float h, float rot, /*float ballRadius*/ Vector2 pos, GameObject block)
    {
        GameObject visualPart = Instantiate(block);
        visualPart.transform.rotation = Quaternion.Euler(0, 0, rot);
        visualPart.transform.localScale = new Vector3(w, h, 1f);
        visualPart.transform.position = pos;
        this.h = h;
        this.w = w;
        rotation = rot;
        SetVerticies();
        SetEdges();
    }
}
