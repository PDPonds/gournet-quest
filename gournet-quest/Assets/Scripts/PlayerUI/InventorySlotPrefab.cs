using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotPrefab : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    int slotIndex;

    Transform lastParent;
    Image img;

    public void SetupSlot(int index)
    {
        slotIndex = index;
    }

    private void Awake()
    {
        img = GetComponent<Image>();
        img.sprite = PlayerManager.Instance.player_Inventory.GetSlot(slotIndex).Item.item_Icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.raycastTarget = false;
        lastParent = transform.parent;
        transform.SetParent(PlayerManager.Instance.uiManager.canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = PlayerManager.Instance.mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(lastParent);
        img.raycastTarget = true;
    }

    public void MoveToInventoryParent()
    {
        PlayerManager.Instance.player_Inventory.SetHandSlot(slotIndex, null);
        lastParent = PlayerManager.Instance.uiManager.slotParent;
    }

    public void SetHandSlot(Transform handSlot)
    {
        lastParent = handSlot.transform;
        PlayerManager.Instance.player_Inventory.SetHandSlot(slotIndex, handSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerManager.Instance.uiManager.ShowItemDiscription(PlayerManager.Instance.player_Inventory.GetSlot(slotIndex));
        }
    }
}
