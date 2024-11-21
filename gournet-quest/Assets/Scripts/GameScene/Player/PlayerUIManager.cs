using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Canvas canvas;

    [Header("===== Inventory =====")]
    [SerializeField] GameObject InventoryPanel;
    public Transform slotParent;
    [SerializeField] GameObject inventorySlotPrefab;
    [Header("- Item Detail")]
    [SerializeField] Image item_Icon;
    [SerializeField] TextMeshProUGUI item_Name;
    [SerializeField] TextMeshProUGUI item_Discription;
    [SerializeField] TextMeshProUGUI item_ItemCount;
    [SerializeField] TextMeshProUGUI item_ItemWeight;
    [Header("- Hand Slot")]
    [SerializeField] List<Transform> allHandSlot = new List<Transform>();
    HandSlot curHandSlotSelected;

    private void Start()
    {
        SelectHandSlot(0);
    }

    public void ToggleInventoryPanel()
    {
        if (InventoryPanel.activeSelf)
        {
            HideItemDiscription();
            if (curHandSlotSelected == null) SelectHandSlot(0);
            InventoryPanel.SetActive(false);
        }
        else
        {
            UpdateInventorySlot();
            InventoryPanel.SetActive(true);
        }
    }

    public void ShowItemDiscription(InventorySlot slot)
    {
        ItemSO item = slot.Item;
        float weight = slot.GetSlotWeight();
        int count = slot.count;

        item_Icon.sprite = item.item_Icon;
        item_Name.text = item.item_Name;
        item_Discription.text = item.item_Discription;
        item_ItemCount.text = count.ToString();
        item_ItemWeight.text = $"{weight} g.";
    }

    void HideItemDiscription()
    {
        item_Icon.sprite = null;
        item_Name.text = string.Empty;
        item_Discription.text = string.Empty;
        item_ItemCount.text = string.Empty;
        item_ItemWeight.text = string.Empty;
    }

    void UpdateInventorySlot()
    {
        ClearInventorySlotParent();
        if (PlayerManager.Instance.player_Inventory.slots.Count > 0)
        {
            for (int i = 0; i < PlayerManager.Instance.player_Inventory.slots.Count; i++)
            {
                InventorySlot slot = PlayerManager.Instance.player_Inventory.slots[i];
                Transform parent = null;
                if (slot.curHandSlot != null) parent = slot.curHandSlot.transform;
                else parent = slotParent;
                GameObject slotObj = Instantiate(inventorySlotPrefab, parent);
                InventorySlotPrefab slotPrefab = slotObj.GetComponent<InventorySlotPrefab>();
                slotPrefab.SetupSlot(i);
            }
        }
    }

    void ClearInventorySlotParent()
    {
        if (slotParent.childCount > 0)
        {
            for (int i = 0; i < slotParent.childCount; i++)
            {
                Destroy(slotParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < allHandSlot.Count; i++)
        {
            Transform handSlot = allHandSlot[i].transform;
            if (handSlot.childCount > 0)
            {
                Destroy(handSlot.GetChild(0).gameObject);
            }
        }

    }

    public void SelectHandSlot(int index)
    {
        if (curHandSlotSelected != null) curHandSlotSelected.HideSelectedBorder();

        if (index < 0 || index >= allHandSlot.Count) return;
        HandSlot handSlot = allHandSlot[index].GetComponent<HandSlot>();
        handSlot.ShowSelectedBorder();
        curHandSlotSelected = handSlot;
    }

}