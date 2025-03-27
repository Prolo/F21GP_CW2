using UnityEngine;

public class RandomLight : Interactable
{
    private Light myLight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (myLight.enabled)
            {
                myLight.enabled = false;
            }
            else
            {
                myLight.enabled = true;
            }
        }
    }

    void Start()
    {
        myLight = GetComponentInChildren<Light>();

        if (Random.value < 0.5f)
        {
            myLight.enabled = true;
        }
        else
        {
            myLight.enabled = false;
        }
    }
}