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
        switch (item.item_Type)
        {
            case ItemType.Equipment:
                EquipmentItem equipment = (EquipmentItem)item;
                InventorySlot equipmentSlot = new InventorySlot(item, count, equipment.maxDurability);
                slots.Add(equipmentSlot);
                break;
            case ItemType.Ingredient:
            case ItemType.EnergyItem:

                if (HasItem(item, out int slotIndex))
                {
                    slots[slotIndex].count += count;
                }
                else
                {
                    InventorySlot slot = new InventorySlot(item, count);
                    slots.Add(slot);
                }

                break;
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

    public void ClearSlot(int slotIndex)
    {
        slots.RemoveAt(slotIndex);
    }

}

[Serializable]
public class InventorySlot
{
    public ItemSO Item;
    public int count;
    public float maxDurability;
    public float curDurability;

    public Transform curHandSlot;

    public InventorySlot(ItemSO item, int count)
    {
        Item = item;
        this.count = count;
    }

    public InventorySlot(ItemSO item, int count, float durability)
    {
        Item = item;
        this.count = count;
        this.maxDurability = durability;
        this.curDurability = durability;
    }

    public float GetSlotWeight()
    {
        float slotWeight = 0f;
        slotWeight = Item.item_Weight * count;
        return slotWeight;
    }

    public bool DecreaseDurabilityAndIsDestroy(float durability)
    {
        curDurability -= durability;
        if (curDurability <= 0f)
        {
            return true;
        }
        return false;
    }

    public void IncreaseDurability(float durability)
    {
        curDurability += durability;
        if (curDurability >= maxDurability)
        {
            curDurability = maxDurability;
        }
    }

}
