using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Represents a triangle with three wp_Points
public class wp_Point 
{
    public string objectID;
    public float X;
    public float Z;

    public wp_Point() { }

    public wp_Point(float X, float Z)
    {
        this.X = X;
        this.Z = Z;
    }

    public wp_RoomCenterData toRCD()
    {
        return Procedural_Script.wp_Dictionary[objectID];
    }
}

public class Triangle
{
    public wp_Point A { get; }
    public wp_Point B { get; }
    public wp_Point C { get; }

    public Triangle(wp_Point a, wp_Point b, wp_Point c)
    {
        A = a;
        B = b;
        C = c;
    }

    // Checks if a given wp_Point lies inside the circumcircle of the triangle
    public bool Iswp_PointInsideCircumcircle(wp_Point wp)
    {
        double ax = A.X - wp.X, ay = A.Z - wp.Z;
        double bx = B.X - wp.X, by = B.Z - wp.Z;
        double cx = C.X - wp.X, cy = C.Z - wp.Z;

        double det = (ax * ax + ay * ay) * (bx * cy - by * cx) -
                     (bx * bx + by * by) * (ax * cy - ay * cx) +
                     (cx * cx + cy * cy) * (ax * by - ay * bx);

        return det > 0;
    }
}

// Bowyer-Watson algorithm for Delaunay triangulation
public static class DelaunayTriangulation
{
    public static List<Triangle> BowyerWatson(List<wp_Point> wp_Points)
    {
        List<Triangle> triangulation = new List<Triangle>();

        // Create a super-triangle that encompasses all wp_Points
        wp_Point p1 = new wp_Point(-1000, -1000);
        wp_Point p2 = new wp_Point(1000, -1000);
        wp_Point p3 = new wp_Point(0, 1000);
        Triangle superTriangle = new Triangle(p1, p2, p3);
        triangulation.Add(superTriangle);

        // Process each wp_Point
        foreach (var wp in wp_Points)
        {
            List<Triangle> badTriangles = triangulation.Where(t => t.Iswp_PointInsideCircumcircle(wp)).ToList();
            HashSet<(wp_Point, wp_Point)> polygon = new HashSet<(wp_Point, wp_Point)>();

            // Identify polygon boundary and remove duplicate edges
            foreach (var triangle in badTriangles)
            {
                AddEdge(polygon, triangle.A, triangle.B);
                AddEdge(polygon, triangle.B, triangle.C);
                AddEdge(polygon, triangle.C, triangle.A);
            }

            // Remove bad triangles
            triangulation.RemoveAll(t => badTriangles.Contains(t));

            // Re-triangulate the polygonal hole
            foreach (var edge in polygon)
            {
                triangulation.Add(new Triangle(edge.Item1, edge.Item2, wp));
            }
        }

        // Remove triangles that contain vertices from the big triangle
        triangulation.RemoveAll(t => t.A == p1 || t.A == p2 || t.A == p3 ||
                                     t.B == p1 || t.B == p2 || t.B == p3 ||
                                     t.C == p1 || t.C == p2 || t.C == p3);

        return triangulation;
    }

    private static void AddEdge(HashSet<(wp_Point, wp_Point)> polygon, wp_Point p1, wp_Point p2)
    {
        var edge = (p1, p2);
        var reverseEdge = (p2, p1);

        if (polygon.Contains(edge)) polygon.Remove(edge);
        else if (polygon.Contains(reverseEdge)) polygon.Remove(reverseEdge);
        else polygon.Add(edge);
    }

    //METHOD TO REFINE THE CONNECTIONS
    public static HashSet<(wp_Point, wp_Point)> GetUniqueConnections(List<Triangle> triangulation)
    {
        HashSet<(wp_Point, wp_Point)> uniqueEdges = new HashSet<(wp_Point, wp_Point)>();

        foreach (var triangle in triangulation)
        {
            AddUniqueConnection(uniqueEdges, triangle.A, triangle.B);
            AddUniqueConnection(uniqueEdges, triangle.B, triangle.C);
            AddUniqueConnection(uniqueEdges, triangle.C, triangle.A);
        }

        return uniqueEdges;
    }

    private static void AddUniqueConnection(HashSet<(wp_Point, wp_Point)> edges, wp_Point p1, wp_Point p2)
    {
        wp_Point first;
        wp_Point second;

        if (p1.X < p2.X || (p1.X == p2.X && p1.Z < p2.Z))
        {
            first = p1;
            second = p2;
        }
        else
        {
            first = p2;
            second = p1;
        }

        var edge = (first, second);

        if (!edges.Contains(edge))
        {
            edges.Add(edge);
        }
    }
}