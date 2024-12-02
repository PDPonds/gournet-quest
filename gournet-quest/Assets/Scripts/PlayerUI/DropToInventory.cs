using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropToInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        InventorySlotPrefab slotPrefab = eventData.pointerDrag.GetComponent<InventorySlotPrefab>();
        if (slotPrefab == null) return;

        slotPrefab.MoveToInventoryParent();
        PlayerManager.Instance.uiManager.HideItemDiscription();

    }
}
