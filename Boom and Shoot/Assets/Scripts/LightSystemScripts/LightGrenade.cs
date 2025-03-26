using UnityEngine;

public class LightGrenade : MonoBehaviour
{
    public float lightDuration = 10f;  
    private Light grenadeLight;
    private Rigidbody rb;
    private bool activated = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grenadeLight = GetComponentInChildren<Light>();
        grenadeLight.enabled = false; // Start with light off
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!activated)  // Ensure it only activates once
        {
            ActivateLight();
        }
    }

    void ActivateLight()
    {
        activated = true;
        grenadeLight.enabled = true;
        Debug.Log("Light Grenade Activated!");

        // Destroy grenade after duration
        Destroy(gameObject, lightDuration);
    }
}
