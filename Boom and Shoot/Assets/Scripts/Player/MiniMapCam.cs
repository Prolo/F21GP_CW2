using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    Dictionary<int, GameObject> objectDict;

    private void Start()
    {
        objectDict = new Dictionary<int, GameObject>();
    }

    private void Update()
    {
        Vector3 dir = playerPos.position - transform.position;

        objectDict.Clear();

        Ray ray = new Ray(transform.position, dir);
        RaycastHit[] hits = Physics.RaycastAll(ray, dir.magnitude);

        foreach (RaycastHit hit in hits) //get the list of gameobjects hit by the way
        {
            if (hit.collider.CompareTag("Roof")) //if the gameobject has the tag "Roof"
            {
                objectDict.Add(hit.collider.gameObject.GetInstanceID(), hit.collider.gameObject); //add it to the list
            }
        }
        foreach (var pair in objectDict)
        {
            pair.Value.GetComponent<MeshRenderer>().enabled = false; //hide all the gameobjects in the list
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(playerPos.position.x, 50, playerPos.position.z);
    }

}
