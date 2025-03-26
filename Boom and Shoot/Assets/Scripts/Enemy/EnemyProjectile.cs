using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Used for the projectiles speed and how long it lasts
    [SerializeField] private float bulletSpeed, lifeTime, bulletDamage;
    

    void Start()
    {
        // Destroy the gameobject after a certain amount of time
        Invoke("DestroyProjectile", lifeTime);
    }

    void FixedUpdate()
    {
        // Move the object at a certain speed
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
            if (collision.tag != "enemy")
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                collision.GetComponent<PlayerControl>().damaged(1, bulletDamage);
                }
                // Destroy the gameobject when it hits a collider
                Destroy(this.gameObject);
            }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
