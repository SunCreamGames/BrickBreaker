using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : Block
{
    protected GameObject spriteBlock;
    override public void SetVerticies(Vector2 pos, float rot)
    {
        verts = new Vector2[4];
        verts[0] = pos + new Vector2(-w / 2, h / 2);
        verts[1] = pos + new Vector2(w / 2, h / 2);
        verts[2] = pos + new Vector2(w / 2, -h / 2);
        verts[3] = pos + new Vector2(-w / 2, -h / 2);
    }
    public override void SetEdges()
    {
        edges = new Edge[4];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new Edge(verts[i], verts[(i + 1) % 4]);
            if (edges[i].rotation == Edge.Rotation.Vertical)
            {
                edges[i].IsRightUp = this.pos.x < edges[i].V1.x;
            }
            else
            {
                edges[i].IsRightUp = !LinAl.isPointUpperThanLine(this.pos, edges[i].V1, edges[i].V2);
            }
        }
    }
 
    public NormalBlock(float w, float h, float rot, Vector2 pos, GameObject block)
    {
        if (block != null)
        {
            spriteBlock = GameObject.Instantiate(block);
            spriteBlock.transform.rotation = Quaternion.Euler(0, 0, rot);
            spriteBlock.transform.localScale = new Vector3(w, h, 1f);
            spriteBlock.transform.position = pos;
        }
        this.pos = pos;
        this.h = h;
        this.w = w;
        RecalculateCollider(pos, rot);
    }
    public NormalBlock(float w, float h, float rot, Vector2 pos, GameObject block, Color color)
    {
        if (block != null)
        {
            spriteBlock = GameObject.Instantiate(block);
            spriteBlock.GetComponent<SpriteRenderer>().color = color;
            spriteBlock.transform.rotation = Quaternion.Euler(0, 0, rot);
            spriteBlock.transform.localScale = new Vector3(w, h, 1f);
            spriteBlock.transform.position = pos;
        }
        this.pos = pos;
        this.h = h;
        this.w = w;
        RecalculateCollider(pos, rot);
    }
    public void RecalculateCollider(Vector2 pos, float rot)
    {
        SetVerticies(pos, rot);
        SetEdges();
    }
}
