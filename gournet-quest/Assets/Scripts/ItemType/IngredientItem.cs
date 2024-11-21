using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Ingredient")]
public class IngredientItem : ItemSO
{ 
    public IngredientItem()
    {
        item_Type = ItemType.Ingredient;
    }
}
