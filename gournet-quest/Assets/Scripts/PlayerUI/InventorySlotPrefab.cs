using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotPrefab : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public int slotIndex;

    [SerializeField] GameObject durabilityBorder;
    [SerializeField] Image durabilityFill;
    [SerializeField] TextMeshProUGUI itemAmountText;

    [HideInInspector] public InventorySO curInventory;

    Transform lastParent;
    Image img;

    public void SetupSlot(int index, InventorySO inventory)
    {
        slotIndex = index;
        img = GetComponent<Image>();
        img.sprite = inventory.GetSlot(slotIndex).Item.item_Icon;
        curInventory = inventory;
        InventorySlot slot = inventory.GetSlot(slotIndex);
        if (slot.Item is EquipmentItem) ShowDurability(inventory);
        else HideDurability();
        UpdateItemAmount(inventory);
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
            PlayerManager.Instance.uiManager.ShowItemDiscription(slotIndex);
        }
    }

    void ShowDurability(InventorySO inventory)
    {
        durabilityBorder.gameObject.SetActive(true);
        UpdateDurability(inventory);
    }

    void HideDurability()
    {
        durabilityBorder.gameObject.SetActive(false);
    }

    public void UpdateDurability(InventorySO inventory)
    {
        float c = inventory.GetSlot(slotIndex).curDurability;
        float m = inventory.GetSlot(slotIndex).maxDurability;
        float p = c / m;
        durabilityFill.fillAmount = p;
    }

    public void UpdateItemAmount(InventorySO inventory)
    {
        int count = inventory.GetSlot(slotIndex).count;
        itemAmountText.text = count.ToString();
        if (count > 1) itemAmountText.gameObject.SetActive(true);
        else itemAmountText.gameObject.SetActive(false);
    }

}
