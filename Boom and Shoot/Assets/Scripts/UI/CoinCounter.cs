using UnityEngine;
using TMPro;

/*
 Updates the coincounter on the UI to reflect how many coins the player has collected.
 */

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private FloatValue coins;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Inventory playerInv;


    public void updateCoinCount()
    {
        display.text = "" + playerInv.score.runtimeValue;
    }
}
