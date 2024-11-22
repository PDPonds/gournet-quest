using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Monster")]
public class MonsterType : ScriptableObject
{
    public string monster_Name;
    public Sprite monster_Icon;
    public GameObject monster_Prefab;
    [Header("- HP")]
    public int monster_HP;
    [Header("- Move")]
    public float monster_moveSpeed;
    [Header("- Attack")]
    public int monster_Damage;
    public float monster_changeTime;
    public float monster_AttackDelay;
    public float monster_AttackRange;
    public List<DropSlot> monster_ItemDrop = new List<DropSlot>();
    public List<InventorySlot> GetDropItem()
    {
        List<InventorySlot> dropSlots = new List<InventorySlot>();
        if (monster_ItemDrop.Count > 0)
        {
            for (int i = 0; i < monster_ItemDrop.Count; i++)
            {
                DropSlot dropSlot = monster_ItemDrop[i];
                if (dropSlot.IsDrop())
                {
                    int count = dropSlot.GetDropCount();
                    if (dropSlot.item is EquipmentItem equipment)
                    {
                        float durability = dropSlot.GetDropDurability();
                        InventorySlot slot = new InventorySlot(dropSlot.item, count, durability);
                        dropSlots.Add(slot);
                    }
                    else
                    {
                        InventorySlot slot = new InventorySlot(dropSlot.item, count);
                        dropSlots.Add(slot);
                    }
                }
            }
        }

        return dropSlots;
    }

}

[Serializable]
public class DropSlot
{
    public ItemSO item;
    [Header("===== Drop Count =====")]
    [Range(0, 100)] public float dropRate;
    [Header("===== Drop Count =====")]
    public int minDropCount;
    public int maxDropCount;
    [Header("===== Drop Durability =====")]
    public float minDurability;
    public float maxDurability;

    public int GetDropCount()
    {
        return UnityEngine.Random.Range(minDropCount, maxDropCount);
    }

    public float GetDropDurability()
    {
        return UnityEngine.Random.Range(minDurability, maxDurability);
    }

    public bool IsDrop()
    {
        int p = UnityEngine.Random.Range(0, 100);
        if (p <= dropRate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
