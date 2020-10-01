using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    Ball ball;
    List<Block> blocks;
    void Start()
    {
        blocks = FindObjectOfType<BlockManager>().blocks;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Block block in blocks)
        {
            foreach (float[] edge in block.edges)
            {
                CheckForCollision(ball.transform.position, ball.radius, edge);
            }
        }
    }
    void CheckForCollision(Vector2 pos, float r, float[] edge)
    {
        if (pos.x + r < edge[3] || pos.x - r > edge[4])
        {
            return;
        }

        float a = edge[0];
        float b = edge[1];
        float c = edge[2];
        float p = pos.x;
        float q = pos.y;

        float D = 4 * (a * c - p * b * b + a * b) * (a * c - p * b * b + a * b)
            - 4 * (b * b + a * a) * (c * c + 2 * b * c * q + q * q * b * b + p * p * b * b - r * r * b * b);
        // Discriminant
        if (D < 0)
        {
            return;
        }
        else if (D == 0)
        {
            float x = (2 * (p * b * b - a * c - a * b) - Mathf.Sqrt(D)) / 2 * b * b * a * a;

            if (x > edge[3] && x < edge[4])
            {
                float x1 = (2 * (p * b * b - a * c - a * b) - Mathf.Sqrt(D)) / 2 * b * b * a * a;
                Vector2 touchPoint = new Vector2(x1, (edge[0] * x1 - edge[2]) / edge[1]);
                ball.SetVelocity(Reflect(touchPoint, edge, ball.velocity));
            }
        }
        else
        {
            float x1 = (2 * (p * b * b - a * c - a * b) - Mathf.Sqrt(D)) / 2 * b * b * a * a;
            float x2 = (2 * (p * b * b - a * c - a * b) + Mathf.Sqrt(D)) / 2 * b * b * a * a;
            if (x1 > edge[3] && x1 < edge[4])
            {
                if (x2 > edge[3] && x2 < edge[4])
                {
                    Vector2 touchPoint1 = new Vector2(x1, (edge[0] * x1 - edge[2]) / edge[1]);
                    Vector2 touchPoint2 = new Vector2(x2, (edge[0] * x2 - edge[2]) / edge[1]);
                    ball.SetVelocity(Reflect(LinAl.Midle(touchPoint1, touchPoint2), edge, ball.velocity));
                }
                else
                {
                    Vector2 touchPoint = new Vector2(x1, (edge[0] * x1 - edge[2]) / edge[1]);
                    ball.SetVelocity(Reflect(touchPoint, edge, ball.velocity));
                }
            }
            else
            {
                if (x2 > edge[3] && x2 < edge[4])
                {
                    Vector2 touchPoint = new Vector2(x2, (edge[0] * x2 - edge[2]) / edge[1]);
                    ball.SetVelocity(Reflect(touchPoint, edge, ball.velocity));
                }
            }
        }
    }

    Vector2 Reflect(Vector2 touch, float[] wall, Vector2 velocity)
    {
        if (wall[1] == 0)
        {
            velocity.x *= -1;
            return velocity;
        }
        if (wall[0] == 0)
        {
            velocity.y *= -1;
            return velocity;
        }
        float speeed = velocity.magnitude;

        float k1 = -wall[0] / wall[1];
        float WallAngle = Mathf.Atan(k1) * Mathf.Rad2Deg;
        float VelocityAngle = Mathf.Atan2(velocity.y,velocity.x) * Mathf.Rad2Deg;
        float newVelocityAngle = 2 * WallAngle - VelocityAngle;

        velocity = new Vector2(1, Mathf.Tan(Mathf.Deg2Rad * newVelocityAngle)).normalized * speeed;
        return velocity;
    }
}