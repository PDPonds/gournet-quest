using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropToFridge_FridgeInventory : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotPrefab slotPrefab = eventData.pointerDrag.GetComponent<InventorySlotPrefab>();
        if (slotPrefab == null) return;
        if (slotPrefab.curInventory == PlayerManager.Instance.fridge_Inventory) return;

        InventorySlot slot = PlayerManager.Instance.player_Inventory.GetSlot(slotPrefab.slotIndex);


        if (slot.Item is IngredientItem)
        {
            PlayerManager.Instance.fridge_Inventory.AddItem(slot.Item, slot.count);
            PlayerManager.Instance.player_Inventory.ClearSlot(slotPrefab.slotIndex);

            PlayerManager.Instance.uiManager.UpdateFridge();
            Destroy(eventData.pointerDrag.gameObject);
        }

    }
}

