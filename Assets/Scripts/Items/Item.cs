using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType //Different categories of items
{
    Equipment,
    KeyItem,
    Consumable,
    Default
}

//Things to fill out about items when made in game
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public string itemName;
}
