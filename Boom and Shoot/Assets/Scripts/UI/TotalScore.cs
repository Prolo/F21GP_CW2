using TMPro;
using UnityEngine;

public class TotalScore : MonoBehaviour
{
    [SerializeField] private FloatValue score, kills, bossKill;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Inventory playerInv;


    public void updateTotalCount()
    {
        display.text = "" + playerInv.score.runtimeValue+(kills.runtimeValue*10)+(bossKill.runtimeValue*100);
    }
}
