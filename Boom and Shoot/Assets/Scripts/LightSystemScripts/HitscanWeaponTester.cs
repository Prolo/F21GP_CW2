using UnityEngine;

public class HitscanWeaponTester : MonoBehaviour
{
    private Transform fireOrigin;
    [SerializeField] private float maxRange = 45f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private float baseAccurateRange = 20f;

    void Start()
    {
        
        fireOrigin = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Fire button pressed!");
            Fire();
        }
    }

    void Fire()
    {

        fireOrigin = Camera.main.transform;

        Ray ray = new Ray(fireOrigin.position, fireOrigin.forward);

        
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxRange, hitMask))  
        {
            Debug.Log("Hit: " + hitInfo.collider.name + " at distance: " + hitInfo.distance);

            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    float illum = enemy.GetComponent<LightDetector>().illuminationLevel;

                    // Introduce a chance to miss based on darkness
                    float hitChance = Mathf.Lerp(0.2f, 1f, illum);
                    // More darkness = higher miss chance
                    if (Random.value > hitChance)  // Random.value is between 0 and 1
                    {
                        Debug.Log("Shot missed due to darkness!");
                        return;  // shot is a complete miss
                    }

                    // Adjust accuracy range based on illumination
                    
                    float darkRange = 0.5f * baseAccurateRange;
                    float dynamicAccurateRange = Mathf.Lerp(darkRange, baseAccurateRange, illum);

                    if (illum > 0.5f)
                    {
                        float extraFactor = (illum - 0.5f) * 2f;
                        dynamicAccurateRange = Mathf.Lerp(baseAccurateRange, maxRange, extraFactor);
                    }
                    dynamicAccurateRange = Mathf.Min(dynamicAccurateRange, maxRange);

                    float distance = hitInfo.distance;
                    if (distance <= dynamicAccurateRange)
                    {
                        enemy.ApplyDamage(baseDamage);
                    }
                    else
                    {
                        float beyondDist = distance - dynamicAccurateRange;
                        float rangeSpan = maxRange - dynamicAccurateRange;
                        float accuracyFactor = 1f - (beyondDist / rangeSpan);
                        int damage = Mathf.RoundToInt(baseDamage * accuracyFactor);
                        if (damage > 0)
                        {
                            enemy.ApplyDamage(damage);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Missed! No hit detected.");
        }
    }
}
