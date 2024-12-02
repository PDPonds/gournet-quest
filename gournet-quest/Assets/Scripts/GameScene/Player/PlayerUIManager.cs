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

    [Header("===== Interactive =====")]
    [SerializeField] TextMeshProUGUI interactiveText;

    [Header("===== Cooking Station =====")]
    [SerializeField] GameObject cookingStationBG;
    [Header("- Frigde")]
    [SerializeField] Button fridgeBut;
    [SerializeField] GameObject fridgeBorder;
    public Transform playerInventoryInFridgeParent;
    public Transform fridgeInventoryParent;
    [Header("- Food Station")]
    [SerializeField] Button foodReseachBut;
    [SerializeField] GameObject foodReseachBorder;

    private void Start()
    {
        SelectHandSlot(0);
    }

    public void ToggleInventoryPanel()
    {
        if (PlayerManager.Instance.isBehavior(PlayerBehavior.UIShowing)) return;

        if (InventoryPanel.activeSelf)
        {
            HideItemDiscription();
            if (curHandSlotSelected == null) SelectHandSlot(0);
            PlayerManager.Instance.SwitchBehavior(PlayerBehavior.Normal);
            InventoryPanel.SetActive(false);

            if (cookingStationBG.activeSelf) HideCookingStation();

        }
        else
        {
            UpdateInventorySlot();
            PlayerManager.Instance.SwitchBehavior(PlayerBehavior.UIShowing);
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

        item_Use_But.gameObject.SetActive(true);
        item_Drop_But.gameObject.SetActive(true);

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
        }
        else if (item is IngredientItem ingredientItem)
        {
            item_Use_But.gameObject.SetActive(false);
            item_DurabilityBorder.gameObject.SetActive(false);
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
        item_DurabilityBorder.SetActive(false);
        item_Use_But.gameObject.SetActive(false);
        item_Drop_But.gameObject.SetActive(false);
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
                slotPrefab.SetupSlot(i, PlayerManager.Instance.player_Inventory);
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

    public void ShowInteractiveUI(string text)
    {
        interactiveText.gameObject.SetActive(true);
        interactiveText.text = text;
    }

    public void HideInteractiveUI()
    {
        interactiveText.gameObject.SetActive(false);
    }

    public void ShowCookingStation()
    {
        PlayerManager.Instance.SwitchBehavior(PlayerBehavior.UIShowing);

        fridgeBut.onClick.RemoveAllListeners();
        foodReseachBut.onClick.RemoveAllListeners();
        fridgeBut.onClick.AddListener(FridgeBut);
        foodReseachBut.onClick.AddListener(FoodReseachBut);
        FridgeBut();

        cookingStationBG.SetActive(true);
    }

    public void HideCookingStation()
    {
        PlayerManager.Instance.SwitchBehavior(PlayerBehavior.Normal);
        cookingStationBG.SetActive(false);
    }

    void FridgeBut()
    {
        UpdateFridge();

        fridgeBut.interactable = false;
        fridgeBorder.gameObject.SetActive(true);

        foodReseachBut.interactable = true;
        foodReseachBorder.SetActive(false);
    }

    void FoodReseachBut()
    {
        fridgeBut.interactable = true;
        fridgeBorder.gameObject.SetActive(false);

        foodReseachBut.interactable = false;
        foodReseachBorder.SetActive(true);
    }

    public void UpdateFridge()
    {
        ClearFrideSlotParent();

        if (PlayerManager.Instance.player_Inventory.slots.Count > 0)
        {
            for (int i = 0; i < PlayerManager.Instance.player_Inventory.slots.Count; i++)
            {
                InventorySlot slot = PlayerManager.Instance.player_Inventory.slots[i];
                GameObject slotObj = Instantiate(inventorySlotPrefab);
                slotObj.transform.SetParent(playerInventoryInFridgeParent);
                InventorySlotPrefab slotPrefab = slotObj.GetComponent<InventorySlotPrefab>();
                slotPrefab.SetupSlot(i, PlayerManager.Instance.player_Inventory);
            }
        }

        if (PlayerManager.Instance.fridge_Inventory.slots.Count > 0)
        {
            for (int i = 0; i < PlayerManager.Instance.fridge_Inventory.slots.Count; i++)
            {
                InventorySlot slot = PlayerManager.Instance.fridge_Inventory.slots[i];
                GameObject slotObj = Instantiate(inventorySlotPrefab);
                slotObj.transform.SetParent(fridgeInventoryParent);
                InventorySlotPrefab slotPrefab = slotObj.GetComponent<InventorySlotPrefab>();
                slotPrefab.SetupSlot(i, PlayerManager.Instance.fridge_Inventory);
            }
        }

    }

    void ClearFrideSlotParent()
    {
        if (playerInventoryInFridgeParent.childCount > 0)
        {
            for (int i = 0; i < playerInventoryInFridgeParent.childCount; i++)
            {
                Destroy(playerInventoryInFridgeParent.GetChild(i).gameObject);
            }
        }

        if (fridgeInventoryParent.childCount > 0)
        {
            for (int i = 0; i < fridgeInventoryParent.childCount; i++)
            {
                Destroy(fridgeInventoryParent.GetChild(i).gameObject);
            }
        }

    }

}