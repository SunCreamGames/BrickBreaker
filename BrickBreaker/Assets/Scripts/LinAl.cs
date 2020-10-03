using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinAl
{
    public enum Reflect
    {
        Vertical, Horizontal, Diagonal
    }
    public static Vector2 Midle(Vector2 v1, Vector2 v2)
    {
        return new Vector2((v2.x - v1.x) / 2f + v1.x, (v2.y - v1.y) / 2f + v1.y);
    }
    public static Vector2 CrossOfTheLines(float[] line1, float[] line2)
    {
        if ((line1[1] == 0 && line2[1] == 0) || (line1[0] == 0 && line2[0] == 0))
        {
            return Vector2.zero;
        }
        else if (line2[1] == 0)
        {
            float x1 = -line2[2] / line2[0];
            float y = -(line1[2] + line1[0] * x1) / line1[1];
            return new Vector2(x1, y);
        }
        else if (line1[1] == 0)
        {
            float x = -line1[2] / line1[0];
            float y1 = -(line2[2] + line2[0] * x) / line2[1];
            return new Vector2(x, y1);
        }
        else if (line1[0] == 0)
        {
            float y = -line1[2] / line1[1];
            float x1 = -(line2[2] + line2[1] * y) / line2[0];
            return new Vector2(x1, y);
        }
        else if (line2[0] == 0)
        {
            float y1 = -line2[2] / line2[1];
            float x = -(line1[2] + line1[1] * y1) / line1[0];
            return new Vector2(x, y1);
        }
        else
        {
            float x = (line2[2] * line1[1] - line1[2] * line2[1]) / (line1[0] * line2[1] - line2[0] * line1[1]);
            float y = -(line1[0] * x + line1[2]) / line1[1];
            return new Vector2(x, y);
        }
    }
    public static bool isPointUpperThanLine(Vector2 point, Vector2 startLine, Vector2 endLine)
    {
        float y;
        float[] line = GetLine(startLine, endLine);
        y = GetY(line, point.x);
        return y < point.y;
    }
    public static float GetY(float[] line, float x)
    {
        return -(line[0] * x + line[2]) / line[1];
    }
    public static float[] GetLine(Vector2 v1, Vector2 v2)
    {
        float[] line = new float[3];
        line[0] = v2.y - v1.y;
        line[1] = v1.x - v2.x;
        line[2] = v1.y * v2.x - v2.y * v1.x;
        return line;
    }

    public static float[] GetPerpendicularLine(Vector2 point, float[] basicLine)
    {
        float[] perpendLine = new float[3];
        if (basicLine[0] == 0)
        {
            float y = -basicLine[2] / basicLine[1];
            Vector2 crossPerpendAndBasic = new Vector2(point.x, y);
            return GetLine(point, crossPerpendAndBasic);
        }
        else if (basicLine[1] == 0)
        {
            float x = -basicLine[2] / basicLine[0];
            Vector2 crossPerpendAndBasic = new Vector2(x, point.y);
            return GetLine(point, crossPerpendAndBasic);
        }
        else
        {
            perpendLine[0] = -basicLine[1] / basicLine[0]; //   -k from y = kx + b 
            perpendLine[2] = -(point.y - perpendLine[0] * point.x); // -b from y = kx + b
            perpendLine[1] = 1;
            // ax+by+c = 0 <=> kx + 1y + b = 0
            return perpendLine;
        }
    }
    public static float GetDistance(Vector2 point, Edge edge)
    {
        float[] basicLine = edge.Line;
        float[] perpendLine = GetPerpendicularLine(point, basicLine);
        Vector2 crossPerpendAndBasic = CrossOfTheLines(perpendLine, basicLine);
        if (crossPerpendAndBasic.x < Mathf.Max(edge.V2.x, edge.V1.x) && crossPerpendAndBasic.x > Mathf.Min(edge.V2.x, edge.V1.x) &&
            crossPerpendAndBasic.y < Mathf.Max(edge.V2.y, edge.V1.y) && crossPerpendAndBasic.y > Mathf.Min(edge.V2.y, edge.V1.y))
        {
            return (point - crossPerpendAndBasic).magnitude;
        }
        else
        {
            return (point - crossPerpendAndBasic).magnitude * 100f;
        }
    }
}
