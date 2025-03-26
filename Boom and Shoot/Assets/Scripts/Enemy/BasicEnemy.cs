using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/*'
 Our basic enemy script, inherits from the EnemyScript for shared functions between all enemies.
 */

public class BasicEnemy : EnemyScript
{
    // Used to determine where the enemy spawns, and where it is targetting for movement and throwing stones
    [SerializeField] private Transform target, home, gun, shotPoint;

    // The projectile used by the cyclops
    [SerializeField] private GameObject projectile;

    // Floats to determine where the cyclops attacks and hunts in, and how long between shots
    [SerializeField] private float huntArea, attackArea, startBetweenShots, rotateSpeed;
    private float timeBtwnShots;

    // check for when the target is in range
    private bool inRange;

    // Cyclop's rigidbody
    private Rigidbody RB;

    private void Start()
    {
        current = EnemyState.idle;
        RB = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Gets the difference between the target position and gun child object position
        Vector3 difference = target.position - gun.transform.position;
        // Sets the z rotation of gun object by getting the x and y difference changing it to degrees
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        // Transforms the z rotation using euler
        // gun.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        // Checks if the target is in the hunt area but not in attack area
        if (Vector3.Distance(transform.position, target.position) <= huntArea && Vector3.Distance(transform.position, target.position) > attackArea)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
        // Checks if player is in attack area
        if (Vector3.Distance(transform.position, target.position) <= attackArea)
        {

            ChangeState(EnemyState.attack);

            // Gets x and y difference between this object and target
            float deltaX = transform.position.x - target.position.x;
            float deltaY = transform.position.z - target.position.z;

            Vector3 move = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            Quaternion rotate = Quaternion.LookRotation(target.position - transform.position);

            // Move the rigibody to the target
            RB.MovePosition(move);
            RB.MoveRotation(rotate);

            // If the timeBtwnShots <= 0 instantiate projectile from shotpoint child object
            if (timeBtwnShots <= 0)
            {      
                // Creates a projectile
                Instantiate(projectile, shotPoint.position, shotPoint.transform.rotation);

                // Set the shot time to start again
                timeBtwnShots = startBetweenShots;
            }
            else
            {
                // Take away from timeBtwnShots everyframe until it reaches 0
                timeBtwnShots -= Time.deltaTime;
            }
        }

        // If the player goes outside attack area reset shot time
        if (Vector2.Distance(transform.position, target.position) > attackArea)
        {
            timeBtwnShots = startBetweenShots;
        }
    }
    void FixedUpdate()
    {
        // If target is in hunt range, move object towards target position
        if (inRange == true)
        {
            Vector3 move = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 
            Quaternion rotate = Quaternion.LookRotation(target.position - transform.position);
            
            // Move the rigibody to the target
            RB.MovePosition(move);
            RB.MoveRotation(rotate);
            // Set state to walk
            ChangeState(EnemyState.walk);            
        }
        else
        {
            
            ChangeState(EnemyState.idle);
        }

    }
    void OnDrawGizmos()
    {
        // Draw hunt and attack area ranges in the editor
        Gizmos.DrawWireSphere(transform.position, huntArea);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackArea);
    }
}
