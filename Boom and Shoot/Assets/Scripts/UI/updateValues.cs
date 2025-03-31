using UnityEngine;

public class updateValues : MonoBehaviour
{
    [SerializeField] private SignalSender hpSignal, pickupSignal, grenadeSignal, scoreSignal, killsSignal, bossSignal, totalSignal;
    private bool cursorLock = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpSignal.Raise();
        pickupSignal.Raise();
        scoreSignal.Raise();
        killsSignal.Raise();
        bossSignal.Raise();
        totalSignal.Raise();
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
