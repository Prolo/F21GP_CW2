using UnityEngine;


/*
 A scriptable object that stores a bool value, true or false, for use by certain objects such as chests or doors.
 */

[CreateAssetMenu(fileName = "BoolValue", menuName = "Scriptable Objects/BoolValue")]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
    // the start and runtime values of the bool
    public bool startValue;
    public bool runtimeValue;

    public void OnAfterDeserialize()
    {
        runtimeValue = startValue;
    }

    public void OnBeforeSerialize()
    {

    }    
}
