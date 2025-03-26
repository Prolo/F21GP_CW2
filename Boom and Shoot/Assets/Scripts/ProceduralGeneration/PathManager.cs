using System.Collections.Generic;
using UnityEngine;

public static class PathManager
{
    public static Queue<(wp_Point, wp_Point)> pathQueue = new Queue<(wp_Point, wp_Point)>();

    public static List<wp_Path_Data> openNodes = new List<wp_Path_Data>();

    public static List<wp_Path_Data> closedNodes = new List<wp_Path_Data>();

    public static List<List<wp_Path_Data>> allPaths = new List<List<wp_Path_Data>>();

    public static bool pathsFinished = false;

    public static bool buildingpaths = false;

    public static bool buildingWalls = false;

    public static bool wallsFinished = false;

    public static bool closingDoors = false;

    public static bool doorsClosed = false;

}
