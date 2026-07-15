using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshUtils
{
    public static List<Vector2> ConvexHull(List<Vector2> points)
    {
        // Remove duplicates
        points = points.Distinct().ToList();
        Debug.Log("MeshUtils: Computing Convex Hull for points: " + string.Join(", ", points));
        if (points.Count <= 1)
        {
            return new List<Vector2>(points);
        }

        // Sort by X then Y
        points.Sort((a, b) =>
        {
            int cmp = a.x.CompareTo(b.x);
            return cmp != 0 ? cmp : a.y.CompareTo(b.y);
        });

        List<Vector2> lower = new List<Vector2>();
        foreach (var p in points)
        {
            while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }

            lower.Add(p);
        }

        List<Vector2> upper = new List<Vector2>();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            var p = points[i];
            while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }

            upper.Add(p);
        }

        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);

        lower.AddRange(upper);
        return lower;
    }

    private static float Cross(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
    }
}
