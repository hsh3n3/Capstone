using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public int itemCount = 0;
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                itemCount += _amount;
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
        int quantity = itemCount;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && Container[i].amount >= 1)
            {
                Container[i].RemoveAmount(_amount);
                itemCount -= _amount;
                break;
            }

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
        itemCount = Container.Count;

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
