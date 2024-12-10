using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
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
    [SerializeField] GameObject menuPrefab;
    [SerializeField] Transform menuParent;
    [SerializeField] GameObject menuInfoBorder;
    [Header("Menu Info")]
    [SerializeField] Image menuInfo_Icon;
    [SerializeField] TextMeshProUGUI menuInfo_MenuName;
    [SerializeField] TextMeshProUGUI menuInfo_MenuType;
    [SerializeField] GameObject menuInfo_MenuDetail;
    [SerializeField] TextMeshProUGUI menuInfo_MenuMaxCost;
    [SerializeField] Image menuInfo_CompletenessCountFill;
    [SerializeField] TextMeshProUGUI menuInfo_CompletenessCountText;
    [SerializeField] Transform menuInfo_IngredientParent;
    [SerializeField] GameObject menuInfo_IngredientPrefab;
    [SerializeField] Button menuInfo_ReseachBut;
    [Header("Menu In Reseact Btn")]
    [SerializeField] Button allMenuBtn;
    [SerializeField] Button appetizerBtn;
    [SerializeField] Button mainDishBtn;
    [SerializeField] Button dessertBtn;
    [SerializeField] Button beverageBtn;
    [Header("- Restaurant Menu Btn")]
    [SerializeField] Button restaurantBut;
    [SerializeField] GameObject restaurantMenuBorder;
    [Header("Menu In Reseact Slot")]
    [SerializeField] Transform mir_MenuParent;
    [Header("Menu In Restaurant Btn")]
    [SerializeField] Button mir_allMenuBtn;
    [SerializeField] Button mir_appetizerBtn;
    [SerializeField] Button mir_mainDishBtn;
    [SerializeField] Button mir_dessertBtn;
    [SerializeField] Button mir_beverageBtn;

    [Header("Cooking Scene")]
    [SerializeField] GameObject loadCookingScenePanel;
    [SerializeField] Image loadCookingSceneFill;

    [Header("===== Restuarant =====")]
    public Button open_and_close_restaurant_but;
    [SerializeField] TextMeshProUGUI open_and_close_text;

    private void Start()
    {
        open_and_close_restaurant_but.onClick.AddListener(ToggleRestaurantState);
        SelectHandSlot(0);
    }

    public void CloseAllUI()
    {
        HideItemDiscription();
        HideInfoBorder();
        if (curHandSlotSelected == null) SelectHandSlot(0);
        PlayerManager.Instance.SwitchBehavior(PlayerBehavior.Normal);
        InventoryPanel.SetActive(false);

        if (cookingStationBG.activeSelf) HideCookingStation();
    }

    public void ToggleInventoryPanel()
    {
        if (!InventoryPanel.activeSelf && !PlayerManager.Instance.isBehavior(PlayerBehavior.UIShowing))
        {
            UpdateInventorySlot();
            PlayerManager.Instance.SwitchBehavior(PlayerBehavior.UIShowing);
            InventoryPanel.SetActive(true);
            return;
        }

        CloseAllUI();

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
        ClearParent(slotParent);
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
        restaurantBut.onClick.RemoveAllListeners();
        restaurantBut.onClick.AddListener(RestaurantMenuBut);
        FridgeBut();

        allMenuBtn.onClick.RemoveAllListeners();
        allMenuBtn.onClick.AddListener(() => AllMenuBtn(menuParent, GameManager.Instance.menus.menus));

        appetizerBtn.onClick.RemoveAllListeners();
        appetizerBtn.onClick.AddListener(() => AppetizerBtn(GameManager.Instance.menus.menus));

        mainDishBtn.onClick.RemoveAllListeners();
        mainDishBtn.onClick.AddListener(() => MainDishBtn(GameManager.Instance.menus.menus));

        dessertBtn.onClick.RemoveAllListeners();
        dessertBtn.onClick.AddListener(() => DessertBtn(GameManager.Instance.menus.menus));

        beverageBtn.onClick.RemoveAllListeners();
        beverageBtn.onClick.AddListener(() => BeverageBtn(GameManager.Instance.menus.menus));

        mir_allMenuBtn.onClick.RemoveAllListeners();
        mir_allMenuBtn.onClick.AddListener(() => AllMenuBtn(mir_MenuParent, GameManager.Instance.curPlayerMenu.GetAllMenus()));

        mir_appetizerBtn.onClick.RemoveAllListeners();
        mir_appetizerBtn.onClick.AddListener(() => AppetizerBtn(GameManager.Instance.curPlayerMenu.GetAllMenus()));

        mir_mainDishBtn.onClick.RemoveAllListeners();
        mir_mainDishBtn.onClick.AddListener(() => MainDishBtn(GameManager.Instance.curPlayerMenu.GetAllMenus()));

        mir_dessertBtn.onClick.RemoveAllListeners();
        mir_dessertBtn.onClick.AddListener(() => DessertBtn(GameManager.Instance.curPlayerMenu.GetAllMenus()));

        mir_beverageBtn.onClick.RemoveAllListeners();
        mir_beverageBtn.onClick.AddListener(() => BeverageBtn(GameManager.Instance.curPlayerMenu.GetAllMenus()));


        cookingStationBG.SetActive(true);
    }

    public void HideCookingStation()
    {
        cookingStationBG.SetActive(false);
        PlayerManager.Instance.SwitchBehavior(PlayerBehavior.Normal);
    }

    void FridgeBut()
    {
        UpdateFridge();
        HideInfoBorder();

        fridgeBut.interactable = false;
        fridgeBorder.gameObject.SetActive(true);

        foodReseachBut.interactable = true;
        foodReseachBorder.SetActive(false);

        restaurantBut.interactable = true;
        restaurantMenuBorder.gameObject.SetActive(false);

    }

    void FoodReseachBut()
    {
        AllMenuBtn(menuParent, GameManager.Instance.menus.menus);

        fridgeBut.interactable = true;
        fridgeBorder.gameObject.SetActive(false);

        foodReseachBut.interactable = false;
        foodReseachBorder.SetActive(true);

        restaurantBut.interactable = true;
        restaurantMenuBorder.gameObject.SetActive(false);
    }

    void RestaurantMenuBut()
    {
        AllMenuBtn(mir_MenuParent, GameManager.Instance.curPlayerMenu.GetAllMenus());

        restaurantBut.interactable = false;
        restaurantMenuBorder.gameObject.SetActive(true);

        fridgeBut.interactable = true;
        fridgeBorder.gameObject.SetActive(false);

        foodReseachBut.interactable = true;
        foodReseachBorder.SetActive(false);
    }

    public void UpdateFridge()
    {
        ClearParent(fridgeInventoryParent);

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

    void ClearParent(Transform parent)
    {
        if (parent.childCount > 0)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }

    void ShowMenuInfo_HasMenuAlready(MenuSlot menuSlot)
    {
        HideInfoBorder();

        menuInfo_Icon.sprite = menuSlot.Menu.menu_Icon;
        menuInfo_MenuName.text = menuSlot.Menu.menu_Name;
        menuInfo_MenuType.text = menuSlot.Menu.menu_Type.ToString();
        menuInfo_MenuMaxCost.text = menuSlot.Menu.menu_Cost.ToString();

        menuInfo_MenuDetail.SetActive(true);
        float c = menuSlot.completenessCount;
        float m = 100f;
        float p = c / m;
        menuInfo_CompletenessCountText.text = (p * 100f).ToString("F0");
        menuInfo_CompletenessCountFill.fillAmount = p;
        menuInfo_MenuMaxCost.text = menuSlot.GetCost().ToString();

        menuInfo_ReseachBut.interactable = true;
        menuInfo_ReseachBut.onClick.RemoveAllListeners();
        menuInfo_ReseachBut.onClick.AddListener(() => Reseach(menuSlot.Menu));


        InitIngredientsInfo(menuSlot.Menu);

        menuInfoBorder.SetActive(true);
    }

    void ShowMenuInfo_CanReseach(Menu menu)
    {
        HideInfoBorder();

        menuInfo_Icon.sprite = menu.menu_Icon;
        menuInfo_MenuName.text = menu.menu_Name;
        menuInfo_MenuType.text = menu.menu_Type.ToString();
        menuInfo_MenuMaxCost.text = menu.menu_Cost.ToString();

        menuInfo_MenuDetail.SetActive(false);

        menuInfo_ReseachBut.interactable = true;
        menuInfo_ReseachBut.onClick.RemoveAllListeners();
        menuInfo_ReseachBut.onClick.AddListener(() => Reseach(menu));

        InitIngredientsInfo(menu);

        menuInfoBorder.SetActive(true);
    }

    void ShowMenuInfo_NoHasIngredient(Menu menu)
    {
        HideInfoBorder();

        menuInfo_Icon.sprite = menu.menu_Icon;
        menuInfo_MenuName.text = menu.menu_Name;
        menuInfo_MenuType.text = menu.menu_Type.ToString();
        menuInfo_MenuMaxCost.text = menu.menu_Cost.ToString();

        menuInfo_MenuDetail.SetActive(false);

        menuInfo_ReseachBut.interactable = false;

        InitIngredientsInfo(menu);

        menuInfoBorder.SetActive(true);
    }

    void Reseach(Menu menu)
    {
        GameManager.Instance.curCookingMenu = menu;
        GameManager.Instance.curCompletness = 100f;
        loadCookingScenePanel.SetActive(true);
        StartCoroutine(GameManager.Instance.LoadLevelAsync(2, loadCookingSceneFill));
    }

    void InitIngredientsInfo(Menu menu)
    {
        if (menuInfo_IngredientParent.childCount > 0)
        {
            for (int i = 0; i < menuInfo_IngredientParent.childCount; i++)
            {
                Destroy(menuInfo_IngredientParent.GetChild(i).gameObject);
            }
        }

        List<IngredientSlot> slots = PlayerManager.Instance.GetAllIngredientsInPlayer();

        if (menu.ingredients.Count > 0)
        {
            for (int i = 0; i < menu.ingredients.Count; i++)
            {
                IngredientItem ingredient = menu.ingredients[i].ingredient;
                int count = menu.ingredients[i].count;

                GameObject obj = Instantiate(menuInfo_IngredientPrefab, menuInfo_IngredientParent);
                IngredientsPrefab prefab = obj.GetComponent<IngredientsPrefab>();
                prefab.Setup(ingredient, count);
                if (slots.Count > 0)
                {
                    for (int j = 0; j < slots.Count; j++)
                    {
                        if (slots[j].ingredient == ingredient)
                        {
                            prefab.Setup(slots[j], count);
                            break;
                        }
                    }
                }
            }
        }

    }

    void HideInfoBorder()
    {
        menuInfoBorder.SetActive(false);
    }

    void AllMenuBtn(Transform parent, List<Menu> menus)
    {
        allMenuBtn.interactable = false;
        appetizerBtn.interactable = true;
        mainDishBtn.interactable = true;
        dessertBtn.interactable = true;
        beverageBtn.interactable = true;

        mir_allMenuBtn.interactable = false;
        mir_appetizerBtn.interactable = true;
        mir_mainDishBtn.interactable = true;
        mir_dessertBtn.interactable = true;
        mir_beverageBtn.interactable = true;

        InitMenuPrefab(parent, menus, menuPrefab);
    }

    void AppetizerBtn(List<Menu> menus)
    {
        allMenuBtn.interactable = true;
        appetizerBtn.interactable = false;
        mainDishBtn.interactable = true;
        dessertBtn.interactable = true;
        beverageBtn.interactable = true;

        mir_allMenuBtn.interactable = true;
        mir_appetizerBtn.interactable = false;
        mir_mainDishBtn.interactable = true;
        mir_dessertBtn.interactable = true;
        mir_beverageBtn.interactable = true;

        InitMenuPrefab(MenuType.Appetizer, menuParent, menus, menuPrefab);

    }

    void MainDishBtn(List<Menu> menus)
    {
        allMenuBtn.interactable = true;
        appetizerBtn.interactable = true;
        mainDishBtn.interactable = false;
        dessertBtn.interactable = true;
        beverageBtn.interactable = true;

        mir_allMenuBtn.interactable = true;
        mir_appetizerBtn.interactable = true;
        mir_mainDishBtn.interactable = false;
        mir_dessertBtn.interactable = true;
        mir_beverageBtn.interactable = true;

        InitMenuPrefab(MenuType.MainCourse, menuParent, menus, menuPrefab);
    }

    void DessertBtn(List<Menu> menus)
    {
        allMenuBtn.interactable = true;
        appetizerBtn.interactable = true;
        mainDishBtn.interactable = true;
        dessertBtn.interactable = false;
        beverageBtn.interactable = true;

        mir_allMenuBtn.interactable = true;
        mir_appetizerBtn.interactable = true;
        mir_mainDishBtn.interactable = true;
        mir_dessertBtn.interactable = false;
        mir_beverageBtn.interactable = true;

        InitMenuPrefab(MenuType.Dessert, menuParent, menus, menuPrefab);
    }

    void BeverageBtn(List<Menu> menus)
    {
        allMenuBtn.interactable = true;
        appetizerBtn.interactable = true;
        mainDishBtn.interactable = true;
        dessertBtn.interactable = true;
        beverageBtn.interactable = false;

        mir_allMenuBtn.interactable = true;
        mir_appetizerBtn.interactable = true;
        mir_mainDishBtn.interactable = true;
        mir_dessertBtn.interactable = true;
        mir_beverageBtn.interactable = false;

        InitMenuPrefab(MenuType.Beverage, menuParent, menus, menuPrefab);
    }

    void InitMenuPrefab(Transform parent, List<Menu> menus, GameObject prefab)
    {
        ClearParent(parent);
        if (menus.Count > 0)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                Menu menu = menus[i];
                GameObject obj = Instantiate(prefab, parent);
                Image img = obj.GetComponent<Image>();
                Button btn = obj.GetComponent<Button>();
                img.sprite = menu.menu_Icon;
                if (parent == menuParent)
                {
                    if (GameManager.Instance.curPlayerMenu.HasMenu(menu, out int index))
                    {
                        img.color = Color.white;
                        MenuSlot menuSlot = GameManager.Instance.curPlayerMenu.GetSlot(index);
                        btn.onClick.AddListener(() => ShowMenuInfo_HasMenuAlready(menuSlot));
                    }
                    else
                    {
                        if (menu.CanReseach())
                        {
                            img.color = Color.gray;
                            btn.onClick.AddListener(() => ShowMenuInfo_CanReseach(menu));
                        }
                        else
                        {
                            img.color = Color.black;
                            btn.onClick.AddListener(() => ShowMenuInfo_NoHasIngredient(menu));
                        }
                    }
                }
                else if (parent == mir_MenuParent)
                {

                }
            }
        }
    }

    void InitMenuPrefab(MenuType type, Transform parent, List<Menu> menus, GameObject prefab)
    {
        ClearParent(parent);
        if (menus.Count > 0)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                Menu menu = menus[i];
                if (menu.menu_Type == type)
                {
                    GameObject obj = Instantiate(prefab, parent);
                    Image img = obj.GetComponent<Image>();
                    Button btn = obj.GetComponent<Button>();
                    img.sprite = menu.menu_Icon;
                    if (GameManager.Instance.curPlayerMenu.HasMenu(menu, out int index))
                    {
                        img.color = Color.white;
                        MenuSlot menuSlot = GameManager.Instance.curPlayerMenu.GetSlot(index);
                        btn.onClick.AddListener(() => ShowMenuInfo_HasMenuAlready(menuSlot));
                    }
                    else
                    {
                        if (menu.CanReseach())
                        {
                            img.color = Color.gray;
                            btn.onClick.AddListener(() => ShowMenuInfo_CanReseach(menu));
                        }
                        else
                        {
                            img.color = Color.black;
                            btn.onClick.AddListener(() => ShowMenuInfo_NoHasIngredient(menu));
                        }
                    }
                }
            }
        }
    }

    void ToggleRestaurantState()
    {
        GameManager.Instance.isRestaurantOpen = !GameManager.Instance.isRestaurantOpen;
        if (GameManager.Instance.isRestaurantOpen)
        {
            open_and_close_text.text = $"Close Restaurant";
        }
        else
        {
            open_and_close_text.text = $"Open Restaurant";
        }
    }

}