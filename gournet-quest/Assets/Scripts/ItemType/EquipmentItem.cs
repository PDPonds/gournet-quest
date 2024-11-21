using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment")]
public class EquipmentItem : ItemSO
{
    public float maxDurability;
    public float durabilityPerUse;
    public EquipmentItem()
    {
        item_Type = ItemType.Equipment;
    }
}
