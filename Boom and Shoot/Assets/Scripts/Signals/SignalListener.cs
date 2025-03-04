using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Listens for signals sent by other gameobjects, then invokes a result in the listening object
 */
public class SignalListener : MonoBehaviour
{
    public SignalSender signal;
    public UnityEvent sEvent;
    public void SignalRaised()
    {
        sEvent.Invoke();
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}