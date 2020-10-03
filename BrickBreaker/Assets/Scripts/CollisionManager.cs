using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    Ball ball;
    List<Block> blocks;
    public void SetBlocks(List<Block> blocks)
    {
        this.blocks = blocks;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Block block in blocks)
        {
            foreach (Vector2 vert in block.verts)
            {
                Gizmos.DrawSphere(new Vector3(vert.x, vert.y, 0f), 0.05f);
            }
            for (int i = 0; i < block.verts.Length; i++)
            {
                Gizmos.DrawLine(new Vector3(block.verts[i % 4].x, block.verts[i % 4].y, 0f),
                    new Vector3(block.verts[(i + 1) % 4].x, block.verts[(i + 1) % 4].y, 0f));
            }
        }
    }
    void FixedUpdate()
    {

        foreach (Block block in blocks)
        {

            //--------------------------------------------------------------------------------------------------//

            // Need to fix doubleReflect. Velocityvector doesn't  help. Maybe I should use shortest normal, idk.//

            //--------------------------------------------------------------------------------------------------//


            //if (Mathf.Abs(ball.Velocity.x) >= Mathf.Abs(ball.Velocity.y))
            //{
            //    if (ball.transform.position.x > block.pos.x)
            //    {
            //        Edge e = block.edges[1];
            //        block.edges[1] = block.edges[0];
            //        block.edges[0] = e;
            //    }
            //    else
            //    {
            //        Edge e = block.edges[3];
            //        block.edges[3] = block.edges[2];
            //        block.edges[2] = block.edges[1];
            //        block.edges[1] = block.edges[0];
            //        block.edges[0] = e;
            //    }
            //}
            //else
            //{
            //    if (ball.transform.position.y > block.pos.y)
            //    {

            //    }
            //    else
            //    {
            //        Edge e = block.edges[2];
            //        block.edges[2] = block.edges[1];
            //        block.edges[1] = block.edges[0];
            //        block.edges[0] = e;
            //    }
            //}
            foreach (Edge edge in block.edges)
            {
                bool a = CheckForCollision(ball.transform.position, ball.radius, /*block*/edge);
                if (a)
                {
                    break;
                }
            }
           
        }
    }
    
    bool CheckForCollision(Vector2 pos, float r, Edge edge)
    {
        if (pos.x + r < Mathf.Min(edge.V2.x,edge.V1.x) ||
            pos.x - r > Mathf.Max(edge.V2.x, edge.V1.x) ||
            pos.y + r < Mathf.Min(edge.V2.y, edge.V1.y) ||
            pos.y - r > Mathf.Max(edge.V2.y, edge.V1.y))
        {
            return false;
        }
        if ((edge.rotation != Edge.Rotation.Vertical)&&(edge.isRightUp && !LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2))||
            (!edge.isRightUp && LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)))
        {
            return false;
        }
        if (edge.rotation == Edge.Rotation.Vertical)
        {
            if(pos.x<edge.V1.x&& edge.isRightUp || (pos.x > edge.V1.x && !edge.isRightUp))
            {
                return false;
            }
        }
        float a = edge.Line[0];
        float b = edge.Line[1];
        float c = edge.Line[2];
        float p = pos.x;
        float q = pos.y;

        float A = 1 + (a / b) * (a / b);
        float B = 2 * a * c / (b * b) - 2 * p + a * q / b;
        float C = p * p + (c / b) * (c / b) + 2 * q * c / b + q * q - r * r;

        float D = B * B - 4 * A * C; // Discriminant
        if (D < 0)
        {
            Debug.Log("Nihuya");
            return false;
        }
        else /*if (D >= 0)*/
        {
            if (edge.rotation == Edge.Rotation.Horizontal)
            {
                ball.SetVelocity(new Vector2(ball.Velocity.x, -ball.Velocity.y));
                ball.transform.position =
                      (ball.transform.position.y > edge.V2.y) ?
                      new Vector3(ball.transform.position.x, edge.V2.y + r + 0.0001f, ball.transform.position.z) :
                      new Vector3(ball.transform.position.x,edge.V2.y - r - 0.0001f, ball.transform.position.z);
                return true;
            }
            else if (edge.rotation == Edge.Rotation.Vertical)
            {
                ball.SetVelocity(new Vector2(-ball.Velocity.x, ball.Velocity.y));
                ball.transform.position =
                    (ball.transform.position.x>edge.V2.x) ? 
                    new Vector3(edge.V2.x + r + 0.0001f,ball.transform.position.y, ball.transform.position.z):
                    new Vector3(edge.V2.x - r - 0.0001f, ball.transform.position.y, ball.transform.position.z);
                return true;
            }
            else
            {
                Debug.Log("OOOO");
                float x = -(B + Mathf.Sqrt(D)) / (2 * A);
                float y = (x * A + C)/ -B;

                ball.SetVelocity(Reflect(new Vector2(x,y), edge, ball.Velocity));
                return true;
            }
        }
    }

    Vector2 Reflect(Vector2 touch, Edge wall, Vector2 velocity)
    {
       
        float speeed = velocity.magnitude;

        float k1 = -wall.Line[0] / wall.Line[1];
        float WallAngle = Mathf.Atan(k1) * Mathf.Rad2Deg;
        float VelocityAngle = Mathf.Atan2(velocity.y,velocity.x) * Mathf.Rad2Deg;
        float newVelocityAngle = 2 * WallAngle - VelocityAngle;

        velocity = new Vector2(Mathf.Tan(Mathf.Deg2Rad * newVelocityAngle), 1f).normalized * speeed;
        return velocity;
    }
}