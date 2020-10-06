using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    Ball ball;
    public List<Block> blocks { get; private set; }
    public event Action<Block> Collision;
    bool hasDestroyedABlock;
    public void SetBlocks(List<Block> blocks)
    {
        this.blocks = blocks;
        if (this.blocks == null || this.blocks.Count == 0)
        {
            this.blocks = new List<Block>();
        }
        blocks.Add(FindObjectOfType<Platform>().Block);
    }
   
    void FixedUpdate()
    {
        hasDestroyedABlock = false;
        for (int i = 0; i < blocks.Count; i++)
        {
            foreach (Edge edge in blocks[i].edges)
            {
                bool a = CheckForCollision(ball.transform.position, ball.radius, edge);
                if (a)
                {
                    if (!hasDestroyedABlock)
                        Collision(blocks[i]);
                    hasDestroyedABlock = true;
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
        if ((edge.rotation != Edge.Rotation.Vertical)&&(edge.IsRightUp && !LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2))||
            (!edge.IsRightUp && LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)))
        {
            return false;
        }
        if (edge.rotation == Edge.Rotation.Vertical)
        {
            if(pos.x<edge.V1.x&& edge.IsRightUp || (pos.x > edge.V1.x && !edge.IsRightUp))
            {
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
                if ((edge.IsRightUp && !LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)) ||
                    (!edge.IsRightUp && LinAl.isPointUpperThanLine(pos, edge.V1, edge.V2)))
                {
                    return false;
                }
                ball.SetVelocity(Reflect(new Vector2(x,y), edge, ball.Velocity));

                ball.transform.position = new Vector2(x, y) + LinAl.GetNormalToEdge(edge) * (r + 0.001f);
                return true;
            }
        }
    }

    Vector2 Reflect(Vector2 touch, Edge wall, Vector2 velocity)
    {
       
        float speed = velocity.magnitude;

        float[] normal = LinAl.GetPerpendicularLine(touch, wall.Line);

        float angleWall =  Mathf.Atan(-wall.Line[0] / wall.Line[1])*Mathf.Rad2Deg;
        
        float angleVelocity = Mathf.Rad2Deg * Mathf.Atan2(velocity.y,velocity.x);
        if ((wall.IsRightUp && LinAl.isPointUpperThanLine(ball.transform.position, wall.V1,wall.V2)) ||(!wall.IsRightUp && !LinAl.isPointUpperThanLine(ball.transform.position, wall.V1, wall.V2)))
        {
        float newAngleVelocity = 2 * angleWall - angleVelocity;
        newAngleVelocity *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(newAngleVelocity), Mathf.Sin(newAngleVelocity)).normalized * speed;
        }
        else
        {
            return Vector2.zero;
        }
    }
}