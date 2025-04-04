using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GunWithAccuracy Gun;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Gun.Shoot();
        }
    }
}
