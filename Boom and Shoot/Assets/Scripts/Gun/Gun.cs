using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private float LaserMaxDistance = 100f; 

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
    }

    public void Shoot()
    {
        Debug.Log("Shooting Attempted");

        if (Time.time >= LastShootTime + ShootDelay) 
        {
            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                Instantiate(ImpactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal)); 
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
