using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Ward
}

public class EquipmentItem : ItemSO
{
    public float maxDurability;
    public float durabilityPerUse;
    public float delayTime;
    public EquipmentType equipment_Type;
    public EquipmentItem()
    {
        item_Type = ItemType.Equipment;
    }

    public virtual void UseItem() { }
}
