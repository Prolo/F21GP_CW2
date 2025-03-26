using UnityEngine;
using TMPro;

/*
 Updates the score counter on the UI to reflect how much score the player has accumulated.
 */

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private FloatValue score;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Inventory playerInv;


    public void updateCoinCount()
    {
        display.text = "" + playerInv.score.runtimeValue;
    }
}
