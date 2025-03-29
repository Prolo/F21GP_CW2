using UnityEditor.PackageManager;
using UnityEngine;

public class GunWithAccuracy : MonoBehaviour
{
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private float LaserMaxDistance = 100f;
    [SerializeField] private float baseDamage = 20f;
    [SerializeField] private float baseAccurateRange = 30f;
    [SerializeField] private FloatValue quiver;
    [SerializeField] private SignalSender ammoUpdate;

    private float LastShootTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (quiver.runtimeValue <= 0)
        {
            Debug.Log("Cannot shoot: No ammo!");
            return;
        }

        if (Time.time >= LastShootTime + ShootDelay)
        {
            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                Debug.Log("Hit: " + hit.collider.name + " at distance: " + hit.distance);
                Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

                quiver.runtimeValue--;
                ammoUpdate.Raise();

                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyScript enemy = hit.collider.GetComponent<EnemyScript>();
                    if (enemy != null)
                    {
                        float illum = enemy.GetComponent<LightDetector>().illuminationLevel;

                        float darkRange = 0.5f * baseAccurateRange;
                        float dynamicAccurateRange = Mathf.Lerp(darkRange, baseAccurateRange, illum);

                        if (illum > 0.5f)
                        {
                            float extraFactor = (illum - 0.5f) * 2f;
                            dynamicAccurateRange = Mathf.Lerp(baseAccurateRange, LaserMaxDistance, extraFactor);
                        }
                        dynamicAccurateRange = Mathf.Min(dynamicAccurateRange, LaserMaxDistance);

                        float distance = hit.distance;
                        if (distance <= dynamicAccurateRange)
                        {
                            enemy.ApplyDamage(baseDamage);

                            EnemyFlashOnHit flasher = hit.collider.GetComponent<EnemyFlashOnHit>();
                            if (flasher != null)
                            {
                                flasher.Flash();
                            }
                        }
                        else
                        {
                            float hitChance = Mathf.Lerp(0.2f, 1f, illum);
                            if (Random.value > hitChance)
                            {
                                Debug.Log("Shot missed due to darkness!");
                                return;
                            }

                            float beyondDist = distance - dynamicAccurateRange;
                            float rangeSpan = LaserMaxDistance - dynamicAccurateRange;
                            float accuracyFactor = 1f - (beyondDist / rangeSpan);
                            float damage = Mathf.RoundToInt(baseDamage * accuracyFactor);
                            if (damage > 0)
                            {
                                enemy.ApplyDamage(damage);

                                EnemyFlashOnHit flasher = hit.collider.GetComponent<EnemyFlashOnHit>();
                                if (flasher != null)
                                {
                                    flasher.Flash();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Missed! No hit detected.");
                quiver.runtimeValue--;
                ammoUpdate.Raise();
            }

            LastShootTime = Time.time;
        }
    }

    private Vector3 GetDirection()
    {
        return transform.forward;
    }
}
