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
        foreach (var item in verts)
        {
            Debug.Log(item.x + " ::: " + item.y);
        }
        return verts;
    }
    public override Edge[] SetEdges()
    {
        edges = new Edge[4];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new Edge(verts[i], verts[(i + 1) % 4]);
            Debug.DrawLine(edges[i].V1, edges[i].V2, Color.white, 15f);
        }
        return edges;
    }
    //override public void GetReflectData(out LinAl.Reflect reflect, out Vector2 pointOfReflect, Vector2 pos, Vector2 prevPos)
    //{
    //    float[] velocityLine;
    //     velocityLine = LinAl.GetLine(pos, prevPos);
    //    if (prevPos.y >= verts[0].y)
    //    {
    //        if (prevPos.x <= verts[0].x)
    //        {
    //            reflect = LinAl.Reflect.Diagonal;
    //            pointOfReflect = verts[0];
    //        }
    //        else if (prevPos.x >= verts[1].x)
    //        {
    //            reflect = LinAl.Reflect.Diagonal;
    //            pointOfReflect = verts[1];
    //        }
    //        else
    //        {
    //            reflect = LinAl.Reflect.Horizontal;
    //            pointOfReflect = LinAl.CrossOfTheLines(
    //                LinAl.GetLine(verts[0], verts[1]),
    //                LinAl.GetLine(pos, prevPos));
    //        }
    //    }
    //    else if (prevPos.y <= verts[2].y)
    //    {
    //        if (prevPos.x <= verts[3].x)
    //        {
    //            reflect = LinAl.Reflect.Diagonal;
    //            pointOfReflect = verts[3];
    //        }
    //        else if (prevPos.x >= verts[2].x)
    //        {
    //            reflect = LinAl.Reflect.Diagonal;
    //            pointOfReflect = verts[2];
    //        }
    //        else
    //        {
    //            reflect = LinAl.Reflect.Horizontal;
    //            pointOfReflect = LinAl.CrossOfTheLines(
    //                LinAl.GetLine(verts[2], verts[3]),
    //                LinAl.GetLine(pos, prevPos));
    //        }
    //    }
    //    else
    //    {
    //        if (prevPos.x <= verts[0].x)
    //        {
    //            reflect = LinAl.Reflect.Vertical;
    //            pointOfReflect = LinAl.CrossOfTheLines(
    //                LinAl.GetLine(verts[0], verts[3]),
    //                LinAl.GetLine(pos, prevPos));
    //        }
    //        else
    //        {
    //            reflect = LinAl.Reflect.Vertical;
    //            pointOfReflect = LinAl.CrossOfTheLines(
    //                LinAl.GetLine(verts[2], verts[1]),
    //                LinAl.GetLine(pos, prevPos));
    //        }
    //    }
    //}

    public NormalBlock(float w, float h, float rot, Vector2 pos, GameObject block)
    {
        GameObject visualPart = GameObject.Instantiate(block);
        visualPart.transform.rotation = Quaternion.Euler(0, 0, rot);
        visualPart.transform.localScale = new Vector3(w, h, 1f);
        visualPart.transform.position = pos;
        this.pos = pos;
        this.h = h;
        this.w = w;
        rotation = rot;
        SetVerticies();
        SetEdges();
    }
}
