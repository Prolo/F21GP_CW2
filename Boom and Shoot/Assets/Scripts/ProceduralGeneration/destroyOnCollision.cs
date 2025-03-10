using UnityEngine;

public class destroyOnCollision : MonoBehaviour
{
    public bool landed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!landed) 
        {
            Debug.LogWarning($"{transform.parent.gameObject.name} at {transform.parent.position} COLLIDED with {other.gameObject.name} and will be deleted!");
            Destroy(transform.parent.gameObject);
        }
    }
}
