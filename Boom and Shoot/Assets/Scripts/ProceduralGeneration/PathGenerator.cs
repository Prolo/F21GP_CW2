using System.Collections.Generic;
using UnityEngine;

public static class PathGenerator
{

    public static void GeneratePaths()
    {
        foreach ((wp_Point start, wp_Point end) in MSTManager.MSTree)
        {
            //Debug.Log("Edge: " + start.objectID + " <--> " + end.objectID);

            List<GameObject> RoomA = GetRoomDoors(PathManager.wp_Dictionary[start.objectID].transform);

            List<GameObject> RoomB = GetRoomDoors(PathManager.wp_Dictionary[end.objectID].transform);






        }
    }

    public static void Astar()
    { 
    
    }





    public static List<GameObject> GetRoomDoors(Transform obj)
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


    public static (GameObject, GameObject) GetClosestDoors(List<GameObject> RoomA, List<GameObject> RoomB)
    {
        GameObject closestDoorA = null;
        GameObject closestDoorB = null;
        float minDistance = float.MaxValue;

        // Loop through every door in RoomA
        foreach (GameObject doorA in RoomA)
        {
            // Loop through every door in RoomB
            foreach (GameObject doorB in RoomB)
            {
                float distance = Vector3.Distance(doorA.transform.position, doorB.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestDoorA = doorA;
                    closestDoorB = doorB;
                }
            }
        }

        return (closestDoorA, closestDoorB);
    }
}
