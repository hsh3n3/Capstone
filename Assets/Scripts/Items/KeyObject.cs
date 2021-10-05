using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New KeyItem Object", menuName = "Inventory System/Items/KeyItems")]

public class KeyObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.KeyItem;
    }

}