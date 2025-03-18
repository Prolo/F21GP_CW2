using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject lightGrenadePrefab; 
    public Transform throwPoint; 
    public float throwForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))  
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(lightGrenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        Debug.Log("Light Grenade Thrown!");
    }
}
