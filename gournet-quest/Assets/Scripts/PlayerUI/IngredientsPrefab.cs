using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPrefab : MonoBehaviour
{
    [SerializeField] Image ingredientsIcon;
    [SerializeField] TextMeshProUGUI ingredientsName;
    [SerializeField] TextMeshProUGUI ingredientsCount;

    public void Setup(IngredientSlot slot, int useCount)
    {
        ingredientsIcon.sprite = slot.ingredient.item_Icon;
        ingredientsName.text = slot.ingredient.item_Name;
        ingredientsCount.text = $"{slot.count} / {useCount}";
        if (slot.count >= useCount) ingredientsCount.color = Color.white;
        else ingredientsCount.color = Color.red;
    }

    public void Setup(IngredientItem ingredient, int useCount)
    {
        ingredientsIcon.sprite = ingredient.item_Icon;
        ingredientsName.text = ingredient.item_Name;
        ingredientsCount.text = $"0 / {useCount}";
        ingredientsCount.color = Color.red;
    }

}
