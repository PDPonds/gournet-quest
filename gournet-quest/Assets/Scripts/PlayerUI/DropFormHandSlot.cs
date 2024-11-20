using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropFormHandSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotPrefab slotPrefab = eventData.pointerDrag.GetComponent<InventorySlotPrefab>();
        slotPrefab.MoveToInventoryParent();
    }
}
