using UnityEngine;
using TMPro;

/*
 Updates the ammo counter on the UI to reflect how much ammo the player has.
 */

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private FloatValue ammo;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Inventory playerInv;


    public void updateAmmoCount()
    {
        display.text = "" + playerInv.ammo.runtimeValue;
    }
}
