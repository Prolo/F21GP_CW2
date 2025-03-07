using System.Collections.Generic;
using UnityEngine;

public class Procedural_Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public static int currentRooms = 0;

    void Start()
    {
        CreateStartRoom();
        CreateFinishRoom();
    }


    void Update()
    {
        if (currentRooms < numberOfRooms)
        {
            AddRoom();           
        }
    }


    void CreateStartRoom()
    {
        Instantiate(startRoom, Vector3.zero, Quaternion.identity);
    }

    void CreateFinishRoom()
    {
        Vector3 finishPosition = new Vector3(dungeonDistance, 0, 0); 
        Instantiate(finishRoom, finishPosition, Quaternion.identity);
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

        Collider roomCollider = newRoom.GetComponent<Collider>();
        Collider[] hitColliders = Physics.OverlapBox(roomCollider.bounds.center, roomCollider.bounds.extents);


        if (hitColliders.Length <= 2)
        {
            Debug.Log($"{gameObject.name} is NOT colliding with anything.");
            newRoom.GetComponent<destroyOnCollision>().landed = true;
            currentRooms = currentRooms + 1;
        }

    }

}
