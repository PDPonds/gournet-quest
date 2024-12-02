using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropToFridge_PlayerInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotPrefab slotPrefab = eventData.pointerDrag.GetComponent<InventorySlotPrefab>();
        if (slotPrefab == null) return;
        if (slotPrefab.curInventory == PlayerManager.Instance.player_Inventory) return;

        InventorySlot slot = PlayerManager.Instance.fridge_Inventory.GetSlot(slotPrefab.slotIndex);


        if (slot.Item is IngredientItem)
        {
            PlayerManager.Instance.player_Inventory.AddItem(slot.Item, slot.count);
            PlayerManager.Instance.fridge_Inventory.ClearSlot(slotPrefab.slotIndex);

            PlayerManager.Instance.uiManager.UpdateFridge();
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}