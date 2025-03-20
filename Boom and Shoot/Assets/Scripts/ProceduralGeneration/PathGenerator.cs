using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    (wp_Point, wp_Point) currentPath;

    public wp_Path_Data currentStart;

    public wp_Path_Data currentEnd;

    [SerializeField]
    public GameObject wpPathPrefab;

    [SerializeField]
    public GameObject FloorTile;

    [SerializeField]
    public GameObject WallTile;

    void Start()
    {
        
    }

    void Update()
    {
        if (Procedural_Script.PathsStarted == true)
        {
            if (PathManager.pathQueue.Count >= 0 && PathManager.pathsFinished == false)
            {
                if (currentPath.Item1 == null && currentPath.Item2 == null)
                {


                    currentPath = PathManager.pathQueue.Dequeue();

                    (currentStart, currentEnd) = GetClosestDoors(GetRoomDoors(currentPath.Item1.toRCD().transform), GetRoomDoors(currentPath.Item2.toRCD().transform));

                    PathManager.openNodes.Add(currentStart);


                }
                else
                {
                    if (PathManager.openNodes.Count > 0) // If there are still nodes to explore
                    {
                        UpdateCost();
                        wp_Path_Data current = GetLowestFCostNode();


                        if (!HasCurrentArrived(current))
                        {
                            PathManager.closedNodes.Add(current);
                            PathManager.openNodes.Remove(current);
                            List<wp_Path_Data> list = current.Expand(wpPathPrefab);
                            PathManager.openNodes.AddRange(list);
                        }
                        else
                        {
                            PathManager.allPaths.Add(SavePath(current));
                            currentPath = (null, null);
                            DestroyNonPathObjects();
                            //PathManager.allNodes.Clear();
                            //DestroyNonPathObjects(PathManager.openNodes);
                            PathManager.closedNodes.Clear();
                            PathManager.openNodes.Clear();

                            if (PathManager.pathQueue.Count == 0)
                            {
                                PathManager.pathsFinished = true;
                            }
                        }

                    }
                    else
                    {
                        //this is bad
                    }
                }

            }
            else if (PathManager.pathsFinished == true && PathManager.buildingpaths == false)
            {
                BuildPaths();
                PathManager.buildingpaths = true;
            }
            else if (PathManager.pathsFinished == true && PathManager.buildingpaths == true && PathManager.buildingWalls == false)
            {
                BuildWalls();
                PathManager.buildingWalls = true;
            }
        
        
        }
    }


    public static List<wp_Path_Data> SavePath(wp_Path_Data currentEnd)
    {
        List<wp_Path_Data> path = new List<wp_Path_Data>();
        wp_Path_Data currentNode = currentEnd;

        while (currentNode != null) // Backtrack from end to start
        {
            currentNode.isPath = true; 
            path.Add(currentNode); // Add current node to the path
            currentNode = currentNode.Parent; // Move to the parent node
        }

        return path; // Return the collected path
    }


    public static void DestroyNonPathObjects()
    {
        wp_Path_Data[] allPaths = FindObjectsByType<wp_Path_Data>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (wp_Path_Data node in allPaths)
        {
            if (!node.isPath && !node.passageNode) 
            {
                Destroy(node.gameObject); 
            }
        }
    }


    public void UpdateCost()
    {
        foreach (wp_Path_Data node in PathManager.openNodes)
        {
            float gCost = Vector3.Distance(currentStart.transform.position, node.transform.position); 
            float hCost = Vector3.Distance(node.transform.position, currentEnd.transform.position);   
            node.fCost = gCost + hCost; 
        }
    }

    //public bool AnyNodeHasArrived()
    //{
    //    foreach (wp_Path_Data node in PathManager.openNodes)
    //    {
    //        float hCost = Vector3.Distance(node.transform.position, currentEnd.transform.position);
    //        if (Mathf.Approximately(hCost, 1f))
    //        {
    //            return true; // At least one node has hCost = 1 meter
    //        }
    //    }
    //    return false; // No node with hCost = 1 meter
    //}

    public bool HasCurrentArrived(wp_Path_Data current)
    {
        float hCost = Vector3.Distance(current.transform.position, currentEnd.transform.position);
        return hCost == 1f;
    }

    public wp_Path_Data GetLowestFCostNode()
    {
        wp_Path_Data lowestNode = null;
        float lowestFCost = float.MaxValue;

        for (int i = 0; i < PathManager.openNodes.Count; i++)
        {
            if (PathManager.openNodes[i].fCost < lowestFCost)
            {
                lowestFCost = PathManager.openNodes[i].fCost;
                lowestNode = PathManager.openNodes[i];
            }
        }

        return lowestNode;
    }


    public List<GameObject> GetRoomDoors(Transform obj)
    {
        List<GameObject> roomDoors = new List<GameObject>();

        foreach (Transform sibling in obj.parent)
        {
            if (sibling.gameObject.name.StartsWith("wp_Door"))
            {
                roomDoors.Add(sibling.gameObject);
            }
        }

        return roomDoors;
    }

    public void BuildPaths()
    {
        GameObject floorPrefab = Resources.Load<GameObject>("FloorTile");

        foreach (List<wp_Path_Data> path in PathManager.allPaths) 
        {
            foreach (wp_Path_Data node in path) 
            {
                Instantiate(FloorTile, node.transform.position, Quaternion.identity);
            }
        }
        

    }


    public void BuildWalls()
    {
        //GameObject wallPrefab = Resources.Load<GameObject>("FloorTile");

        foreach (List<wp_Path_Data> path in PathManager.allPaths)
        {
            foreach (wp_Path_Data node in path)
            {
                node.CreateWall(WallTile);
            }
        }


    }


    public (wp_Path_Data, wp_Path_Data) GetClosestDoors(List<GameObject> RoomA, List<GameObject> RoomB)
    {
        GameObject closestDoorA = null;
        GameObject closestDoorB = null;
        wp_Path_Data wpdoorA = null;
        wp_Path_Data wpdoorB = null;
        float minDistance = float.MaxValue;

        // Loop through every door in RoomA
        foreach (GameObject doorA in RoomA)
        {
            wpdoorA = doorA.GetComponentInChildren<wp_Path_Data>();
            // Loop through every door in RoomB
            foreach (GameObject doorB in RoomB)
            {
                wpdoorB = doorB.GetComponentInChildren<wp_Path_Data>();

                float distance = Vector3.Distance(doorA.transform.position, doorB.transform.position);

                if (distance < minDistance && wpdoorA.isDoor == false && wpdoorB.isDoor == false)
                {
                    minDistance = distance;
                    closestDoorA = doorA;
                    closestDoorB = doorB;
                }
            }
        }

        wpdoorA = closestDoorA.GetComponentInChildren<wp_Path_Data>();
        wpdoorA.isDoor = true;
        wpdoorB = closestDoorB.GetComponentInChildren<wp_Path_Data>();
        wpdoorB.isDoor = true;

        return (closestDoorA.GetComponentInChildren<wp_Path_Data>(), closestDoorB.GetComponentInChildren<wp_Path_Data>());
    }
}
