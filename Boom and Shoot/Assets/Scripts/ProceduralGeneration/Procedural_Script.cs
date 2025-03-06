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

    void Start()
    {
        CreateStartRoom();
        CreateFinishRoom();
    }


    void Update()
    {
        
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
}
