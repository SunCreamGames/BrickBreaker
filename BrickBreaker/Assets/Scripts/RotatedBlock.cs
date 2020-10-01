using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedBlock : Block
{
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
    override  public Vector2[] SetVerticies()
    {
        verts = new Vector2[4];
        Vector2[] verts1 = new Vector2[4];
        float angle = Mathf.Rad2Deg * Mathf.Atan2(h / 2, w / 2);
        float radius = (new Vector2(w / 2, h / 2)).magnitude;
        verts[0] = new Vector2(
            radius * Mathf.Cos(Mathf.Deg2Rad * (angle + rotation)),
            radius * Mathf.Sin(Mathf.Deg2Rad * (angle + rotation)));
        verts[1] = new Vector2(
            radius * Mathf.Cos(Mathf.Deg2Rad * (180 - angle + rotation)),
            radius * Mathf.Sin(Mathf.Deg2Rad * (180 - angle + rotation)));
        verts[2] = pos - verts[0];
        verts[3] = pos - verts[1];

        verts1[1] = verts1[3] = verts1[2] = verts1[0] = verts[0];
        for (int i = 0; i < verts.Length; i++)
        {
            if (verts[i].y > verts1[1].y)
            {
                verts1[1] = verts[i];
            }
            if (verts[i].y < verts1[3].y)
            {
                verts1[3] = verts[i];
            }
            if(verts[i].x > verts1[2].x)
            {
                verts1[2] = verts[i];
            }
            if(verts[i].x < verts1[0].x)
            {
                verts1[0] = verts[i];
            }
        }
        verts = verts1;
        return verts1;
    }
    override public bool CheckForCollision(Vector2 pos)
    {
        if (!(pos.x > verts[2].x || pos.x < verts[0].x ||
             pos.y < verts[3].y || pos.y > verts[1].y))
            return false;
        else
        {
            if (pos.x < verts[1].x && pos.x < verts[3].x)
            {
                if (!LinAl.isPointUpperThanLine(pos, verts[0], verts[1]) && LinAl.isPointUpperThanLine(pos, verts[3], verts[0]))
                {
                    return true;
                }
            }
            if (pos.x < verts[1].x && pos.x > verts[3].x)
            {
                if (!LinAl.isPointUpperThanLine(pos, verts[0], verts[1]) && LinAl.isPointUpperThanLine(pos, verts[2], verts[3]))
                {
                    return true;
                }
            }
            if (pos.x > verts[1].x && pos.x > verts[3].x)
            {
                if (!LinAl.isPointUpperThanLine(pos, verts[1], verts[2]) && LinAl.isPointUpperThanLine(pos, verts[3], verts[0]))
                {
                    return true;
                }
            }

            if (pos.x > verts[1].x && pos.x < verts[3].x)
            {
                if (!LinAl.isPointUpperThanLine(pos, verts[1], verts[2]) && LinAl.isPointUpperThanLine(pos, verts[2], verts[3]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    override public void GetReflectData(out float[] line, out Vector2 pointOfReflect, Vector2 pos, Vector2 prevPos)
    {
        float[] wall, velocityDirectionLine;
        velocityDirectionLine = LinAl.GetLine(pos, prevPos);
        if (prevPos.x < verts[1].x && prevPos.y>verts[0].y)
        {
            wall = LinAl.GetLine(verts[0], verts[1]);
        }
        else if (prevPos.x > verts[1].x && prevPos.y>verts[0].y)
        {
            wall = LinAl.GetLine(verts[1], verts[2]);
        }
        else if (prevPos.x > verts[3].x && prevPos.y<verts[2].y)
        {
            wall = LinAl.GetLine(verts[2], verts[3]);
        }
        else
        {
            wall = LinAl.GetLine(verts[0], verts[3]);
        }
        pointOfReflect = LinAl.CrossOfTheLines(wall, velocityDirectionLine);
        line = wall; 
    }
    public RotatedBlock(float w, float h, float rot, Vector2 pos, GameObject block)
    {
        GameObject visualPart = Instantiate(block);
        visualPart.transform.localScale = new Vector3(w, h, 1f);
        visualPart.transform.position = pos;
        visualPart.transform.rotation = Quaternion.Euler(0, 0, rot);
        this.h = h;
        this.w = w;
        this.rotation = rot;
        SetVerticies();
        SetEdges();
    }

}
