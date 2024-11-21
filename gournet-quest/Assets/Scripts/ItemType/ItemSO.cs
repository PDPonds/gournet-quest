using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment, Ingredient, EnergyItem
}

public class ItemSO : ScriptableObject
{
    public int item_ID;
    public string item_Name;
    public string item_Discription;
    public Sprite item_Icon;
    public float item_Weight;
    public ItemType item_Type;

}
