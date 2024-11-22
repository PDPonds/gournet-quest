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
    [SerializeField] GameObject item_DurabilityBorder;
    [SerializeField] Image item_DurabilityFill;
    [SerializeField] Button item_Use_But;
    [SerializeField] Button item_Drop_But;

    [Header("- Hand Slot")]
    [SerializeField] List<Transform> allHandSlot = new List<Transform>();
    [HideInInspector] public HandSlot curHandSlotSelected;

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

    public void ShowItemDiscription(int slotIndex)
    {
        InventorySlot slot = PlayerManager.Instance.player_Inventory.GetSlot(slotIndex);
        ItemSO item = slot.Item;
        float weight = slot.GetSlotWeight();
        int count = slot.count;

        item_Icon.sprite = item.item_Icon;
        item_Name.text = item.item_Name;
        item_Discription.text = item.item_Discription;
        item_ItemCount.text = count.ToString();
        item_ItemWeight.text = $"{weight} g.";
        if (item is EquipmentItem eq)
        {
            item_DurabilityBorder.gameObject.SetActive(true);
            float c = slot.curDurability;
            float m = slot.maxDurability;
            float p = c / m;
            item_DurabilityFill.fillAmount = p;
            item_Use_But.gameObject.SetActive(false);
        }
        else if (item is EnergyItem energyItem)
        {
            item_DurabilityBorder.gameObject.SetActive(false);
            item_Use_But.gameObject.SetActive(true);
        }
        else if (item is IngredientItem ingredientItem)
        {
            item_DurabilityBorder.gameObject.SetActive(false);
            item_Use_But.gameObject.SetActive(true);
            item_Use_But.onClick.RemoveAllListeners();
            item_Use_But.onClick.AddListener(() => UseItem(slotIndex));
        }
        item_Drop_But.onClick.AddListener(DropItem);
    }

    public void HideItemDiscription()
    {
        item_Icon.sprite = null;
        item_Name.text = string.Empty;
        item_Discription.text = string.Empty;
        item_ItemCount.text = string.Empty;
        item_ItemWeight.text = string.Empty;
    }

    public void UpdateInventorySlot()
    {
        ClearInventorySlotParent();
        if (PlayerManager.Instance.player_Inventory.slots.Count > 0)
        {
            for (int i = 0; i < PlayerManager.Instance.player_Inventory.slots.Count; i++)
            {
                InventorySlot slot = PlayerManager.Instance.player_Inventory.slots[i];
                GameObject slotObj = Instantiate(inventorySlotPrefab);
                if (slot.curHandSlot != null) slotObj.transform.SetParent(slot.curHandSlot.transform);
                else slotObj.transform.SetParent(slotParent);
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

    void UseItem(int slotIndex)
    {
        InventorySlot slot = PlayerManager.Instance.player_Inventory.GetSlot(slotIndex);
        if (slot.Item is EnergyItem enegyItem)
        {
            bool isDestroy = PlayerManager.Instance.player_Inventory.RemoveItemAndIsDestroy(slot.Item, 1);
            UpdateInventorySlot();
            HideItemDiscription();
            if (!isDestroy) ShowItemDiscription(slotIndex);
        }
    }

    void DropItem()
    {

    }

}