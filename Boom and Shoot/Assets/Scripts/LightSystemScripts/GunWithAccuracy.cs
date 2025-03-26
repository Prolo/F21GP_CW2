using UnityEditor.PackageManager;
using UnityEngine;

public class GunWithAccuracy : MonoBehaviour
{
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private float LaserMaxDistance = 100f;
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private float baseAccurateRange = 30f;
    [SerializeField] private FloatValue quiver;

    private float LastShootTime;
    private LineRenderer laserLine;

    private void Start()
    {
        laserLine = gameObject.AddComponent<LineRenderer>();
        laserLine.startWidth = 0.02f;
        laserLine.endWidth = 0.02f;
        laserLine.material = new Material(Shader.Find("Unlit/Color"));
        laserLine.material.color = Color.red;
    }

    private void Update()
    {
        UpdateLaserPointer();

        if (Input.GetKeyDown(KeyCode.G))
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

                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
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
                            // Always hit within range
                            enemy.ApplyDamage(baseDamage);
                        }
                        else
                        {
                            // Outside accurate range will apply chance-based miss
                            float hitChance = Mathf.Lerp(0.2f, 1f, illum);
                            if (Random.value > hitChance)
                            {
                                Debug.Log("Shot missed due to darkness!");
                                return;
                            }

                            float beyondDist = distance - dynamicAccurateRange;
                            float rangeSpan = LaserMaxDistance - dynamicAccurateRange;
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
                quiver.runtimeValue--;
            }

            LastShootTime = Time.time;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z));
            direction.Normalize();
        }
        return direction;
    }

    private void UpdateLaserPointer()
    {
        if (laserLine == null) return;

        Vector3 direction = transform.forward;
        Vector3 laserEndPoint;

        if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, LaserMaxDistance, Mask))
        {
            laserEndPoint = hit.point;
        }
        else
        {
            laserEndPoint = BulletSpawnPoint.position + direction * LaserMaxDistance;
        }

        laserLine.SetPosition(0, BulletSpawnPoint.position);
        laserLine.SetPosition(1, laserEndPoint);
    }
}
