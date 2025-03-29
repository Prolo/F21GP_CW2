using System.Collections;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack

}
public class EnemyScript : MonoBehaviour
{
    // The enemies core stats
    [Header("Stats")]
    [SerializeField] public int attack;
    [SerializeField] public string enemyName;
    [SerializeField] public float HP, speed;
    [SerializeField] public FloatValue maxHealth, currLevel, enemiesKilled;
    [SerializeField] public SignalSender updateKills;
    [SerializeField] private LightDetector lightDetector;


    // Events that the enemy triggers
    [Header("Events")]
    private Vector3 spawnPoint;
    [SerializeField] public EnemyState current;


    private void OnEnable()
    {
        // Sets the enemies HP
        HP = maxHealth.startValue + currLevel.runtimeValue-1;
        lightDetector = GetComponent<LightDetector>();
    }

    public virtual void ApplyDamage(float damage)
    {


        // Reduces the enemy HP
        HP -= damage;


        // If the enemy is dead, set it to drop loot, and disable the gameobject
        if (HP <= 0)
        {
            enemiesKilled.runtimeValue += 1;
            updateKills.Raise();
            this.gameObject.SetActive(false);
        }

    }

    public void onHit(Rigidbody enemy, float time, float damage)
    {
        StartCoroutine(DamageCo(enemy, time, damage));
    }

    private IEnumerator DamageCo(Rigidbody enemy, float time, float damage)
    {
        if (enemy != null)
        {
            // After a set amount of time, the knockback stops and the enemy can resume moving normally
            yield return new WaitForSeconds(time);
            ApplyDamage(damage);
            enemy.linearVelocity = Vector3.zero;            
        }
    }
    public void ChangeState(EnemyState state)
    {
        // Change current state
        if (current != state)
        {
            current = state;
        }
    }

}
