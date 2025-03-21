using UnityEngine;
using TMPro;

/*
 Updates the grenade counter on the UI to reflect how many grenades the player has.
 */

public class GrenadeCounter : MonoBehaviour
{
    [SerializeField] private FloatValue grenades;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Inventory playerInv;


    public void updateGrenadeCount()
    {
        display.text = "" + playerInv.grenades.runtimeValue;
    }
}
