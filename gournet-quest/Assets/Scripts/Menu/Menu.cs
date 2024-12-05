using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Menu/Menu")]
public class Menu : ScriptableObject
{
    public string menu_Name;
    public Sprite menu_Icon;
    public MenuType menu_Type;

    public List<IngredientSlot> ingredients = new List<IngredientSlot>();
    public List<Process> processes = new List<Process>();

    public int menu_Cost;

    public bool CanReseach()
    {
        int count = 0;
        List<IngredientSlot> slots = PlayerManager.Instance.GetAllIngredientsInPlayer();

        if (slots.Count > 0)
        {
            for (int j = 0; j < ingredients.Count; j++)
            {
                ItemSO useIngredient = ingredients[j].ingredient;
                int useCount = ingredients[j].count;
                for (int i = 0; i < slots.Count; i++)
                {
                    ItemSO slotIngredient = slots[i].ingredient;
                    int slotCount = slots[i].count;
                    if (slotIngredient == useIngredient && slotCount >= useCount)
                    {
                        count++;
                        break;
                    }
                }
            }
        }

        return count == ingredients.Count;
    }

}

[Serializable]
public class IngredientSlot
{
    public IngredientItem ingredient;
    public int count;
}

public enum MenuType
{
    Appetizer , MainCourse , Dessert , Beverage
}