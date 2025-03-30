using UnityEngine;

public class updateValues : MonoBehaviour
{
    [SerializeField] private SignalSender hpSignal, pickupSignal, grenadeSignal, scoreSignal, killsSignal, bossSignal, totalSignal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpSignal.Raise();
        pickupSignal.Raise();
        scoreSignal.Raise();
        killsSignal.Raise();
        bossSignal.Raise();
        totalSignal.Raise();
    }

}
