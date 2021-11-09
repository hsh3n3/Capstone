using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }    
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }

    public void RemoveItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && Container[i].amount >= 1)
            {
                Container[i].RemoveAmount(_amount);
                break;
            }

        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = null;
            Container[i].amount = 0;

        }

    }

    public bool RemoveItemCheck(ItemObject _item)
    {
        bool isGood = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && Container[i].amount >= 1)
            {
                isGood = true;
                break;
            }
            if (Container[i].item == _item && Container[i].amount <= 0)
            {
                isGood = false;
                break;
            }

        }

        return isGood;

    }
    public void update()
    {

    }
}



[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void RemoveAmount(int value)
    {
        amount -= value;
    }
}
