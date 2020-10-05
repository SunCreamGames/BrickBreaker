using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedBlock : Block
{
    public override void SetEdges()
    {
        edges = new Edge[4];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new Edge(verts[i], verts[(i + 1) % 4]);
            edges[i].isRightUp = !LinAl.isPointUpperThanLine(this.pos, edges[i].V1, edges[i].V2);
            
        }
    }
    override public void SetVerticies(Vector2 pos, float rot)
    {
        verts = new Vector2[4];
        Vector2[] verts1 = new Vector2[4];
        float angle = Mathf.Rad2Deg * Mathf.Atan2(h / 2, w / 2);
        float radius = (new Vector2(w / 2, h / 2)).magnitude;
        verts[0] = pos + new Vector2(
            radius * Mathf.Cos(Mathf.Deg2Rad * (angle + rot)),
            radius * Mathf.Sin(Mathf.Deg2Rad * (angle + rot)));
        verts[1] = pos + new Vector2(
            radius * Mathf.Cos(Mathf.Deg2Rad * (180 - angle + rot)),
            radius * Mathf.Sin(Mathf.Deg2Rad * (180 - angle + rot)));
        verts[2] = pos + new Vector2(
         radius * Mathf.Cos(Mathf.Deg2Rad * (180 + angle + rot)),
         radius * Mathf.Sin(Mathf.Deg2Rad * (180 + angle + rot)));
        verts[3] = pos + new Vector2(
            radius * Mathf.Cos(Mathf.Deg2Rad * (360 - angle + rot)),
            radius * Mathf.Sin(Mathf.Deg2Rad * (360 - angle + rot)));
    }
    public RotatedBlock(float w, float h, float rot, Vector2 pos, GameObject block)
    {
        GameObject visualPart = GameObject.Instantiate(block);
        visualPart.transform.localScale = new Vector3(w, h, 1f);
        visualPart.transform.position = pos;
        visualPart.transform.rotation = Quaternion.Euler(0, 0, rot);
        this.h = h;
        this.w = w;
        this.pos = pos;
            RecalculateCollider(pos, rot);
    }
    void RecalculateCollider(Vector2 pos, float rot)
    {
        SetVerticies(pos, rot);
        SetEdges();
    }
}
