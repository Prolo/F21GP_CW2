using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class Procedural_Script : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> rooms;

    [SerializeField]
    public GameObject startRoom;

    [SerializeField]
    public GameObject finishRoom;

    [SerializeField]
    public int dungeonDistance = 100;

    [SerializeField]
    public int numberOfRooms = 1;

    public int currentRooms = 0;

    public List<GameObject> wp_RoomCenters = new List<GameObject>();

    private bool allRoomsInPlace = false;
    private bool delanayTrianglesStarted = false;
    private bool refinningConnectionsStarted = false;
    private bool MSTStarted = false;
    public static bool PathsStarted = false;

    public static Dictionary<string, wp_RoomCenterData> wp_Dictionary = new Dictionary<string, wp_RoomCenterData>();

    void Start()
    {
        CreateStartRoom();
        CreateFinishRoom();
    }

    void Update()
    {


        if (currentRooms < numberOfRooms && allRoomsInPlace == false)
        {
            AddRoom();
        }
        else
        {
            allRoomsInPlace = true;
        }

        if (delanayTrianglesStarted == false && allRoomsInPlace == true)
        {
            foreach (var kvp in wp_Dictionary)
            {
                DelaunayManager.points.Add(kvp.Value.wpTowp_Point());
            }

            DelaunayManager.RunTriangulationAsync();
            delanayTrianglesStarted = true;
        }
        else if (DelaunayManager.IsTriangulationComplete == true && refinningConnectionsStarted == false)
        {
            DelaunayManager.RefineResults();
            refinningConnectionsStarted = true;
        }
        else if (DelaunayManager.IsTriangulationComplete == true && DelaunayManager.isRefinationCompleted == true && MSTStarted == false)
        {

            MSTManager.RunTriangulationAsync();
            MSTStarted = true;

            //bool hasDuplicates = HasDuplicateConnections(DelaunayManager.uniqueConnections);

            //if (hasDuplicates)
            //{
            //    Debug.Log("Duplicate connections found!");
            //}
            //else
            //{
            //    Debug.Log("No duplicate connections.");
            //}

            //DrawUniqueConnections();
        }
        else if (DelaunayManager.IsTriangulationComplete == true && DelaunayManager.isRefinationCompleted == true && MSTManager.IsTreeComplete == true && PathsStarted == false)
        {
            PathsStarted = true;
            //PathManager.wp_Dictionary = this.wp_Dictionary;
            //PathGenerator.GeneratePaths();
            //var a = MSTManager.MSTree;

            //DrawUniqueConnections();

        }
        
    }


    void CreateStartRoom()
    {
        GameObject sRoom = Instantiate(startRoom, Vector3.zero, Quaternion.identity);

        wp_RoomCenterData wp_sRoom = sRoom.GetComponentInChildren<wp_RoomCenterData>();

        wp_Dictionary.Add(wp_sRoom.objectID, wp_sRoom);

        //startRoomId = wp_sRoom.objectID;

        //wp_StartRoom = wp_sRoom.wpTowp_Point();
    }

    void CreateFinishRoom()
    {
        Vector3 finishPosition = new Vector3(dungeonDistance, 0, 0);

        GameObject fRoom = Instantiate(finishRoom, finishPosition, Quaternion.identity);

        wp_RoomCenterData wp_sRoom = fRoom.GetComponentInChildren<wp_RoomCenterData>();

        wp_Dictionary.Add(wp_sRoom.objectID, wp_sRoom);
    }


    Vector3 GetRandomPosition(float dungeonDistance)
    {
        float xMin = 0;
        float xMax = dungeonDistance;
        float zMin = dungeonDistance / 4f;
        float zMax = - dungeonDistance / 4f;

        int randomX = Mathf.RoundToInt(Random.Range(xMin, xMax));
        int randomZ = Mathf.RoundToInt(Random.Range(zMin, zMax));

        return new Vector3(randomX, 0, randomZ);
    }

    GameObject GetRandomRoom()
    {
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogWarning("Room list is empty or not assigned!");
            return null;
        }

        return rooms[Random.Range(0, rooms.Count)];
    }

    void AddRoom()
    {
        GameObject randomRoom = GetRandomRoom();

        Vector3 randomPosition = GetRandomPosition(dungeonDistance);

        GameObject newRoom = Instantiate(randomRoom, randomPosition, Quaternion.identity);

        Collider roomCollider = newRoom.transform.Find("Floor").GetComponent<Collider>();

        Collider[] hitColliders = Physics.OverlapBox(roomCollider.bounds.center, roomCollider.bounds.extents);


        if (hitColliders.Length <= 2)
        {
            Debug.Log($"{gameObject.name} is NOT colliding with anything.");

            newRoom.transform.Find("Floor").GetComponent<destroyOnCollision>().landed = true;

            currentRooms = currentRooms + 1;
            wp_RoomCenterData wp_NewRoom = newRoom.GetComponentInChildren<wp_RoomCenterData>();

            wp_Dictionary.Add(wp_NewRoom.objectID, wp_NewRoom);
        }

    }

    public static void DrawUniqueConnections()
    {
        foreach (var edge in MSTManager.MSTree)
        {
            DrawRay(edge.Item1, edge.Item2);
        }
    }

    public static void DrawRay(wp_Point p1, wp_Point p2)
    {
        GameObject lineObj = new GameObject($"Edge_{p1.objectID}_{p2.objectID}");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.positionCount = 2;

        lr.SetPosition(0, new Vector3(p1.X, 0, p1.Z));
        lr.SetPosition(1, new Vector3(p2.X, 0, p2.Z));

        lr.material = new Material(Shader.Find("Sprites/Default")); 
        lr.startColor = Color.green;
        lr.endColor = Color.green;
    }

    //FOR TESTING, DELETE AFTER
    public static bool HasDuplicateConnections(HashSet<(wp_Point, wp_Point)> uniqueConnections)
    {
        HashSet<(wp_Point, wp_Point)> seenEdges = new HashSet<(wp_Point, wp_Point)>();

        foreach (var edge in uniqueConnections)
        {
            var reverseEdge = (edge.Item2, edge.Item1);

            if (seenEdges.Contains(reverseEdge))
            {
                return true; // duplicate conection
            }

            seenEdges.Add(edge);
        }

        return false; // No duplicates 
    }

}
