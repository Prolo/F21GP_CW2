using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;

public class DelaunayManager : MonoBehaviour
{
    public static List<wp_Point> points = new List<wp_Point>();
    public static List<Triangle> results;
    public static bool IsTriangulationComplete { get; private set; } = false; // Completion flag

    void Start()
    {
        RunTriangulationAsync();
    }

    public static async void RunTriangulationAsync()
    {
        IsTriangulationComplete = false; // Reset flag
        Debug.Log("Starting Delaunay Triangulation...");

        results = await Task.Run(() =>
        {
            return DelaunayTriangulation.BowyerWatson(points);
        });

        IsTriangulationComplete = true; // Set flag when done
        Debug.Log("Triangulation Completed!");

        Procedural_Script.DrawDelaunayTriangles();
    }
}
