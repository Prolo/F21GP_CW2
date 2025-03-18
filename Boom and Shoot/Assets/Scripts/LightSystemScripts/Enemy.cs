using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LightDetector lightDetector; 
    public int health = 100; 

    void Start()
    {
        lightDetector = GetComponent<LightDetector>(); 
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage! Remaining Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has been eliminated!");
        Destroy(gameObject); 
    }
}
