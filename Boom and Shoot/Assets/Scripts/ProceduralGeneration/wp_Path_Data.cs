using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class wp_Path_Data : MonoBehaviour
{
    public wp_Path_Data Parent;
    public float fCost;
    public bool isPath = false;
    public bool isDoor = false;
    public bool passageNode = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public List<wp_Path_Data> Expand(GameObject wpPathPrefab)
    {
        List<wp_Path_Data> list = new List<wp_Path_Data>(); 

        Vector3 north = transform.position + Vector3.forward; // (0, 0, 1)
        Vector3 south = transform.position + Vector3.back;    // (0, 0, -1)
        Vector3 east = transform.position + Vector3.right;   // (1, 0, 0)
        Vector3 west = transform.position + Vector3.left;    // (-1, 0, 0)

        if (!Physics.CheckSphere(north, 0.4f))
        {
            GameObject newPath = Instantiate(wpPathPrefab, north, Quaternion.identity);

            wp_Path_Data pathData = newPath.GetComponent<wp_Path_Data>(); // Get wp_Path_Data component

            if (pathData != null)
            {
                pathData.Parent = this; 
                list.Add(pathData); 
            }
        }


        if (!Physics.CheckSphere(south, 0.4f))
        {
            GameObject newPath = Instantiate(wpPathPrefab, south, Quaternion.identity);

            wp_Path_Data pathData = newPath.GetComponent<wp_Path_Data>(); // Get wp_Path_Data component

            if (pathData != null)
            {
                pathData.Parent = this;
                list.Add(pathData);
            }
        }


        if (!Physics.CheckSphere(east, 0.4f))
        {
            GameObject newPath = Instantiate(wpPathPrefab, east, Quaternion.identity);

            Instantiate(wpPathPrefab, east, Quaternion.identity);

            wp_Path_Data pathData = newPath.GetComponent<wp_Path_Data>(); // Get wp_Path_Data component

            if (pathData != null)
            {
                pathData.Parent = this;
                list.Add(pathData);
            }
        }


        if (!Physics.CheckSphere(west, 0.4f))
        {
            GameObject newPath = Instantiate(wpPathPrefab, west, Quaternion.identity);

            wp_Path_Data pathData = newPath.GetComponent<wp_Path_Data>(); // Get wp_Path_Data component

            if (pathData != null)
            {
                pathData.Parent = this;
                list.Add(pathData);
            }
        }


        return list; // Return all successfully created paths
    }
}
