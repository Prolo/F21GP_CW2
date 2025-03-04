using UnityEngine;

/*
 Useful for a variety of values that might need adjusted on many objects at once, I.E HP
 */

[CreateAssetMenu(fileName = "FLoatValue", menuName = "Scriptable Objects/FLoatValue")]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    // The start and runtime values of the object
    public float startValue;
    public float runtimeValue;

    public void OnAfterDeserialize()
    {
        runtimeValue = startValue;
    }

    public void OnBeforeSerialize()
    {

    }
}
