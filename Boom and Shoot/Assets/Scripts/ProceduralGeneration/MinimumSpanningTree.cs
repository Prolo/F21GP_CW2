using System;
using System.Collections.Generic;
using UnityEngine;

public static class MinimumSpanningTree
{
    public static HashSet<(wp_Point, wp_Point)> PimsAlgorithm()
    {
        HashSet<(wp_Point, wp_Point)> mstEdges = new HashSet<(wp_Point, wp_Point)>();
        HashSet<wp_Point> visitedNodes = new HashSet<wp_Point>();
        List<(wp_Point, wp_Point, float)> edgeList = new List<(wp_Point, wp_Point, float)>();

        // Ensure we have connections to process
        if (DelaunayManager.uniqueConnections.Count == 0)
        {
            Debug.LogError("No connections found in DelaunayManager.uniqueConnections!");
            return mstEdges;
        }

        // Start with the first edge in the HashSet
        var firstEdge = GetFirstEdge(DelaunayManager.uniqueConnections);
        wp_Point startNode = firstEdge.Item1;
        wp_Point secondNode = firstEdge.Item2;

        // Add both nodes to the visited set
        visitedNodes.Add(startNode);
        visitedNodes.Add(secondNode);

        // Add the starting edge to MST
        mstEdges.Add((startNode, secondNode));

        // Initialize edge list with all edges connected to the starting nodes
        foreach (var edge in DelaunayManager.uniqueConnections)
        {
            if (visitedNodes.Contains(edge.Item1) || visitedNodes.Contains(edge.Item2))
            {
                float distance = GetDistance(edge.Item1, edge.Item2);
                edgeList.Add((edge.Item1, edge.Item2, distance));
            }
        }

        // Process MST
        while (edgeList.Count > 0 && visitedNodes.Count < GetUniqueNodeCount())
        {
            // Sort edges manually (smallest weight first)
            edgeList.Sort((a, b) => a.Item3.CompareTo(b.Item3));

            var (p1, p2, weight) = edgeList[0]; // Pick smallest edge
            edgeList.RemoveAt(0); // Remove from list

            wp_Point newNode;
            if (!visitedNodes.Contains(p1))
            {
                newNode = p1;
            }
            else if (!visitedNodes.Contains(p2))
            {
                newNode = p2;
            }
            else
            {
                // Both nodes are already visited, skip this edge
                continue;
            }

            // Add to MST
            mstEdges.Add((p1, p2));
            visitedNodes.Add(newNode);

            // Add new edges from this node
            foreach (var edge in DelaunayManager.uniqueConnections)
            {
                bool edgeAlreadyVisited = visitedNodes.Contains(edge.Item1) && visitedNodes.Contains(edge.Item2);

                if (!edgeAlreadyVisited && (edge.Item1 == newNode || edge.Item2 == newNode))
                {
                    float distance = GetDistance(edge.Item1, edge.Item2);
                    edgeList.Add((edge.Item1, edge.Item2, distance));
                }
            }
        }

        return mstEdges;
    }

    private static (wp_Point, wp_Point) GetFirstEdge(HashSet<(wp_Point, wp_Point)> connections)
    {
        foreach (var edge in connections)
        {
            return edge; // Returns the first edge found
        }
        return default;
    }

    private static int GetUniqueNodeCount()
    {
        HashSet<wp_Point> uniqueNodes = new HashSet<wp_Point>();

        foreach (var edge in DelaunayManager.uniqueConnections)
        {
            uniqueNodes.Add(edge.Item1);
            uniqueNodes.Add(edge.Item2);
        }

        return uniqueNodes.Count;
    }

    private static float GetDistance(wp_Point p1, wp_Point p2)
    {
        return Vector2.Distance(new Vector2(p1.X, p1.Z), new Vector2(p2.X, p2.Z));
    }
}