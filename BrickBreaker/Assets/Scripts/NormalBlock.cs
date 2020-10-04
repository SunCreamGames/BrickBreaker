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
    public override Edge[] SetEdges()
    {
        edges = new Edge[4];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new Edge(verts[i], verts[(i + 1) % 4]);
            if (edges[i].rotation == Edge.Rotation.Vertical)
            {
                edges[i].isRightUp = this.pos.x < edges[i].V1.x;
            }
            else
            {
                edges[i].isRightUp = !LinAl.isPointUpperThanLine(this.pos, edges[i].V1, edges[i].V2);
            }
            Debug.DrawLine(edges[i].V1, edges[i].V2, Color.white, 15f);
        }
        return edges;
    }
 
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
