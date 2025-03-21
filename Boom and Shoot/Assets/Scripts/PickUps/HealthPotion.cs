using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This script handles healing potions of all sizes
*/

public class HealthPotion : Powerup
{
    // Players current health and max health
    [SerializeField] private FloatValue playerHealth, lifeContainers;
    
    // The amount to increase the players HP by
    [SerializeField] private float amountToIncrease, rotationSpeed = 100f;
    
    [SerializeField] public bool active = true;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            // Add the amount of health to the runtime value of the player health
            playerHealth.runtimeValue += amountToIncrease;
            // Set the player health to max if the hp potion gives health over max
            if(playerHealth.runtimeValue > lifeContainers.runtimeValue * 2f)
            {
                playerHealth.runtimeValue = lifeContainers.runtimeValue * 2f;
            }
            // Raise the pickup signal and destroy the object
            pickUpSignal.Raise();
            active = false;
            Destroy(this.gameObject);
            
        }
    }
}
