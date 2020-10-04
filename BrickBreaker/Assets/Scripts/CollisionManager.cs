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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    foreach (Block block in blocks)
    //    {
    //        foreach (Vector2 vert in block.verts)
    //        {
    //            Gizmos.DrawSphere(new Vector3(vert.x, vert.y, 0f), 0.05f);
    //        }
    //        for (int i = 0; i < block.verts.Length; i++)
    //        {
    //            Gizmos.DrawLine(new Vector3(block.verts[i % 4].x, block.verts[i % 4].y, 0f),
    //                new Vector3(block.verts[(i + 1) % 4].x, block.verts[(i + 1) % 4].y, 0f));
    //        }
    //    }
    //}
    void FixedUpdate()
    {
        foreach (Block block in blocks)
        {
            foreach (Edge edge in block.edges)
            {
                bool a = CheckForCollision(ball.transform.position, ball.radius, edge);
                if (a)
                {
                    Debug.DrawLine(edge.V1, edge.V2, Color.red, 0.5f);
                    break;
                }
                Debug.DrawLine(edge.V1, edge.V2, Color.white, 0.5f);
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
            Debug.LogError(edge.V1);
            Debug.LogError(edge.V2);
            return false;
        }
        if (edge.rotation == Edge.Rotation.Vertical)
        {
            if(pos.x<edge.V1.x&& edge.isRightUp || (pos.x > edge.V1.x && !edge.isRightUp))
            {
            Debug.Log("B");
                return false;
            }
        }
        float a = edge.Line[0];
        float b = edge.Line[1];
        float c = edge.Line[2];
        float p = pos.x;
        float q = pos.y;

        //float A = 1 + (a / b) * (a / b);
        //float B = 2 * a * c / (b * b) - 2 * p + a * q / b;
        //float C = p * p + (c / b) * (c / b) + 2 * q * c / b + q * q - r * r;

        //float D = B * B - 4 * A * C; // Discriminant
        float A = 1 + Mathf.Pow(b/a, 2);
        float B = 2 * b * c / Mathf.Pow(a, 2) + 2 * b * p / a - 2 * q;
        float C = Mathf.Pow(c / a, 2) + 2 * p * c / a + Mathf.Pow(p, 2) + Mathf.Pow(q, 2) - Mathf.Pow(r, 2);
        float D = B * B - 4 * A * C;
        
        if (D < 0)
        {
            return false;
        }
        else
        {
            Debug.Log(edge.isRightUp);
            Debug.Log(LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2));
            Debug.Log(LinAl.isPointUpperThanLine(pos, edge.V2, edge.V1));
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
                float y1 = -(B + Mathf.Sqrt(D)) / (2 * A);
                float x1 = (y1 * b + c)/ -a;
                float y2 = -(B - Mathf.Sqrt(D)) / (2 * A);
                float x2 = (y2 * b + c)/ -a;
                float y = y1 + (y2 - y1) / 2;
                float x = x1 + (x2 - x1) / 2;
                if ((edge.isRightUp && !LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)) ||
                    (!edge.isRightUp && LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)))
                {
                    return false;
                }
                ball.SetVelocity(Reflect(new Vector2(x,y), edge, ball.Velocity));

                ball.transform.position = new Vector2(x, y) + LinAl.GetNormalToEdge(edge)/* ball.Velocity.normalized */* (r + 0.001f);
                return true;
            }
        }
    }

    Vector2 Reflect(Vector2 touch, Edge wall, Vector2 velocity)
    {
       
        float speed = velocity.magnitude;

        float[] normal = LinAl.GetPerpendicularLine(touch, wall.Line);

        float angleWall =  Mathf.Atan(-wall.Line[0] / wall.Line[1])*Mathf.Rad2Deg;
        Debug.Log($"angleWall градусы : {angleWall}");
        
        float angleVelocity = Mathf.Rad2Deg * Mathf.Atan2(velocity.y,velocity.x);
        Debug.Log($"angleVelocity : {angleVelocity}");
        //if (wall.isRightUp)
        //{
        //    return false;
        //}
        if ((wall.isRightUp && LinAl.isPointUpperThanLine(ball.transform.position, wall.V1,wall.V2)) ||(!wall.isRightUp && !LinAl.isPointUpperThanLine(ball.transform.position, wall.V1, wall.V2)))
        {
        float newAngleVelocity = 2 * angleWall - angleVelocity;
        Debug.Log($"new : {newAngleVelocity}");
        newAngleVelocity *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(newAngleVelocity), Mathf.Sin(newAngleVelocity)).normalized * speed;
        }
        else
        {
            Debug.Log("HUETA KAKAYA-TO");
            return Vector2.zero;
        }
    }
}