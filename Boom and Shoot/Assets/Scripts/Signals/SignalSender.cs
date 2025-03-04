using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sends a signal, that other gameobjects can listen for to trigger specific events
 */

[CreateAssetMenu]
public class SignalSender : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].SignalRaised();
        }
    }
    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }
    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }
}