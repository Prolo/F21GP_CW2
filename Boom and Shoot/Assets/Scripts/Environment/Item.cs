using UnityEngine;

/*
 Scriptable object that carries basic item information
 */

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Variables")]
    
    public string desc;
    public bool key, bigKey, coinBag, smallCoinBag;
    
    
}
