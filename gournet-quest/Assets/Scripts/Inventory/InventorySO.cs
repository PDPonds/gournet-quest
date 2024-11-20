using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public float inventory_maxWeight;

    public void AddItem(ItemSO item, int count)
    {
        if (HasItem(item, out int slotIndex))
        {
            slots[slotIndex].count += count;
        }
        else
        {
            InventorySlot slot = new InventorySlot(item, count);
            slots.Add(slot);
        }
    }

    public bool RemoveItem(ItemSO item, int count)
    {
        if (HasItem(item, out int slotIndex))
        {
            int inSlotCount = slots[slotIndex].count;
            if (count > inSlotCount)
            {
                slots[slotIndex].count -= count;
            }
            else if (count == inSlotCount)
            {
                slots.RemoveAt(slotIndex);
            }
        }

        return false;
    }

    public bool HasItem(ItemSO item, out int slotIndex)
    {
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];
                ItemSO item_Slot = slot.Item;
                if (item == item_Slot)
                {
                    slotIndex = i;
                    return true;
                }
            }
        }
        slotIndex = -1;
        return false;
    }

    public float GetCurrentWeightInInventory()
    {
        float weight = 0f;

        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];
                ItemSO item_Slot = slot.Item;
                int item_count = slot.count;
                float inSlotWeight = item_Slot.item_Weight * item_count;
                weight += inSlotWeight;
            }
        }

        return weight;
    }

    public float GetWeightOver()
    {
        float overCount = 0f;
        float weight = GetCurrentWeightInInventory();
        if (weight > inventory_maxWeight)
        {
            overCount = weight - inventory_maxWeight;
        }
        return overCount;
    }

    public void SetHandSlot(int slotIndex, Transform handSlot)
    {
        if (slotIndex < slots.Count)
        {
            slots[slotIndex].curHandSlot = handSlot;
        }
    }

    public InventorySlot GetSlot(int slotIndex)
    {
        if (slotIndex < slots.Count)
        {
            return slots[slotIndex];
        }

        return null;
    }

}

[Serializable]
public class InventorySlot
{
    public ItemSO Item;
    public int count;

    public Transform curHandSlot;

    public InventorySlot(ItemSO item, int count)
    {
        Item = item;
        this.count = count;
    }

    public float GetSlotWeight()
    {
        float slotWeight = 0f;
        slotWeight = Item.item_Weight * count;
        return slotWeight;
    }

}
