using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An inheritable object type that checks when the player is within range of something that can be interacted with.
 */

public class Interactable : MonoBehaviour
{
    public bool active;
    public SignalSender on;

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            on.Raise();
            active = true;
        }
    }

    public virtual void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            on.Raise();
            active = false;
        }
    }

}
