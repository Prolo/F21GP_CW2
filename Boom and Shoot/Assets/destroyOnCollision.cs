using UnityEngine;


public class destroyOnCollision : MonoBehaviour
{
    public bool landed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (landed == false)
        {
            Debug.LogWarning($"{gameObject.name} at {transform.position} COLLIDED with {other.gameObject.name} and will be deleted!");
            Destroy(gameObject, 0);
        }

        //Debug.LogWarning($"{gameObject.name} at {transform.position} COLLIDED with {other.gameObject.name} and will be deleted!");
        //Destroy(gameObject, 0);
        //Procedural_Script.currentRooms = Procedural_Script.currentRooms - 1;

    }
}
