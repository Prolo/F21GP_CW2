using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles the players Arrow count, max and ensures it cannot go over the max value.
 */

public class ArrowCount : Powerup
{
    public Inventory playerInv;
    public FloatValue quiver;
    public float maxArrows;

    public void Start()
    {
        pickUpSignal.Raise();
        maxArrows = quiver.runtimeValue-1;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player") && collision.isTrigger)
        {
            // If player enters trigger, increase arrows floatvalue by 1
            playerInv.ammo.runtimeValue += 1;
            // Raise signal
            pickUpSignal.Raise();
            Destroy(this.gameObject);
            // Check to make sure arrows dont go over max
            if (playerInv.ammo.runtimeValue > maxArrows)
            {
                playerInv.ammo.runtimeValue = maxArrows;
                pickUpSignal.Raise();
            }
        }
    }
}
