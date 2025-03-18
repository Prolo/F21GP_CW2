using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class MSTManager
{
    public static bool IsTreeComplete = false; // Completion flag

    public static HashSet<(wp_Point, wp_Point)> MSTree = new HashSet<(wp_Point, wp_Point)>();


    public static async void RunTriangulationAsync()
    {
        IsTreeComplete = false; // Reset flag
        Debug.Log("Starting Delaunay Triangulation...");

        MSTree = await Task.Run(() =>
        {
            return MinimumSpanningTree.PimsAlgorithm();
        });

        IsTreeComplete = true;

        PathManager.pathQueue = new Queue<(wp_Point, wp_Point)>(MSTree);

        Debug.Log("Triangulation Completed!");
    }

}
