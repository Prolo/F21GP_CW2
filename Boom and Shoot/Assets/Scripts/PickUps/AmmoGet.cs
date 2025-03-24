using UnityEngine;

public class AmmoGet : Powerup
{
    // Players current health and max health
    [SerializeField] private FloatValue quiver;

    // The amount to increase the players HP by
    [SerializeField] private float maxAmmo, amountToIncrease, rotationSpeed = 100f;

    [SerializeField] public bool active = true;


    private void Start()
    {
        maxAmmo = quiver.startValue * 2;
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
            quiver.runtimeValue += amountToIncrease;
            // Set the player health to max if the hp potion gives health over max
            if (quiver.runtimeValue > maxAmmo)
            {
                quiver.runtimeValue = maxAmmo;
            }
            // Raise the pickup signal and destroy the object
            pickUpSignal.Raise();
            active = false;
            Destroy(this.gameObject);

        }
    }
}
