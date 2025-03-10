using UnityEngine;

public class wp_RoomCenterData : MonoBehaviour
{
    public string objectID;

    public float X;
    public float Z;

    private void Awake()
    {
        objectID = System.Guid.NewGuid().ToString();
        X = transform.position.x;
        Z = transform.position.z;
    }


    public wp_Point wpTowp_Point()
    {
        wp_Point wp = new wp_Point();

        wp.objectID = this.objectID;
        wp.X = this.X;
        wp.Z = this.Z;
        

        return wp;
    }


}
