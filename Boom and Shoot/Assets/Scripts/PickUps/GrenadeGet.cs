using UnityEngine;

public class GrenadeGet : Powerup
{
    // Players current health and max health
    [SerializeField] private FloatValue grenades;

    // The amount to increase the players HP by
    [SerializeField] private float maxGrenades, amountToIncrease, rotationSpeed = 100f;

    [SerializeField] public bool active = true;


    private void Start()
    {
        maxGrenades = grenades.startValue;
    }
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            // Add the amount of health to the runtime value of the player health
            grenades.runtimeValue += amountToIncrease;
            // Set the player health to max if the hp potion gives health over max
            if (grenades.runtimeValue > maxGrenades)
            {
                grenades.runtimeValue = maxGrenades;
            }
            // Raise the pickup signal and destroy the object
            pickUpSignal.Raise();
            active = false;
            Destroy(this.gameObject);

        }
    }
}
