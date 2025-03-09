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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public wp_Point wpTowp_Point()
    {
        wp_Point wp = new wp_Point();

        wp.objectID = this.objectID;
        wp.X = this.X;
        wp.Z = this.Z;
        

        return wp;
    }

    //public wp_Point wpTowp_Point(float X, float Y)
    //{
    //    wp_Point wp = new()
    //    {
    //        X = this.X,
    //        Y = this.Y
    //    };

    //    return wp;
    //}
}
