using UnityEngine;

public class ScoreGet : Powerup
{
    [SerializeField] Inventory playerInv;
    [SerializeField] private int value;
    [SerializeField] private float rotationSpeed = 100f;
    void Start()
    {
        pickUpSignal.Raise();
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //Debug.Log("Touched");
            //if player enters trigger add a certain amount of coins equal to the value
            playerInv.score.runtimeValue += value;
            //raise signal
            pickUpSignal.Raise();
            // removes the coin object so it can't be picked up multiple times
            Destroy(this.gameObject);
        }
    }
}
