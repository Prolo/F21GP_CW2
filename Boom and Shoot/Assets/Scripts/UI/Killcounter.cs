using TMPro;
using UnityEngine;

public class Killcounter : MonoBehaviour
{
    [SerializeField] private FloatValue kills;
    [SerializeField] private TextMeshProUGUI display;

    public void updateKillCount()
    {
        display.text = "" + kills.runtimeValue;
    }
}
