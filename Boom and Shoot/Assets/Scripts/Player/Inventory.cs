using UnityEngine;
using System.Collections.Generic;

/*
 The players inventory, stores their items from the dungeon
 */


[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    // current item that player has collected, for display purposes
    public Item currItem;
    public List<Item> items = new List<Item>();
    public FloatValue keys, bigKeys, score, ammo, grenades;    

    public void OnEnable()
    {
        // resets the inventory on new start
        items.Clear();
        keys.runtimeValue = 0;
        bigKeys.runtimeValue = 0;
        score.runtimeValue = 0;
        ammo.runtimeValue = ammo.startValue;
        grenades.runtimeValue = grenades.startValue;
        
    }

    public void AddItem(Item item)
    {
        if (item.key)        
            keys.runtimeValue++;
        
        if (item.bigKey)
            bigKeys.runtimeValue++;

        if (item.coinBag)
        {
            score.runtimeValue = score.runtimeValue + 50;            
        }

        if (item.smallCoinBag)
        {
            score.runtimeValue = score.runtimeValue + 10;
        }

    }

    public bool ItemCheck(Item item)
    {
        // checks if you already have that item
        if (items.Contains(item))
            return true;

        return false;
    }
}
