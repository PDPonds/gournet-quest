using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Sprite border_Sprite;
    [SerializeField] Image img;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        InventorySlotPrefab slotPrefab = eventData.pointerDrag.GetComponent<InventorySlotPrefab>();
        InventorySlot slot = PlayerManager.Instance.player_Inventory.GetSlot(slotPrefab.slotIndex);
        ItemSO item = slot.Item;
        if (transform.childCount == 0 && item is EquipmentItem)
        {
            slotPrefab.SetHandSlot(transform);
        }
    }

    public void ShowSelectedBorder()
    {
        img.sprite = border_Sprite;
        img.color = new Color(1, 1, 1, 1);
    }

    public void HideSelectedBorder()
    {
        img.sprite = null;
        img.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }

}
