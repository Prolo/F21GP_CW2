using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;

public class DelaunayManager
{
    public static List<wp_Point> points = new List<wp_Point>();
    public static List<Triangle> results = new List<Triangle>();
    public static HashSet<(wp_Point, wp_Point)> uniqueConnections = new HashSet<(wp_Point, wp_Point)>();

    public static bool IsTriangulationComplete = false; // Completion flag
    public static bool isRefinationCompleted = false; //refine flag

    //void Start() //this is never triggering, I am calling the method in Procedural
    //{
    //    RunTriangulationAsync();
    //    RefineResults();
        
    //}

    public void Reset()
    {
        points = new List<wp_Point>();
        results = new List<Triangle>();
        uniqueConnections = new HashSet<(wp_Point, wp_Point)>();
        IsTriangulationComplete = false;
        isRefinationCompleted = false;
    }

    public static async void RunTriangulationAsync()
    {
        IsTriangulationComplete = false; // Reset flag
        Debug.Log("Starting Delaunay Triangulation...");

        results = await Task.Run(() =>
        {
            return DelaunayTriangulation.BowyerWatson(points);
        });

        IsTriangulationComplete = true;

        Debug.Log("Triangulation Completed!");
    }

    public static async void RefineResults()
    {
        uniqueConnections = await Task.Run(() =>
        {
            return DelaunayTriangulation.GetUniqueConnections(results);
        });

        Debug.Log("Refine Completed!");

        isRefinationCompleted = true;


    }
}
