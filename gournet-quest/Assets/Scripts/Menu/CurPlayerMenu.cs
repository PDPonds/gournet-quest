using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cur Player Menu")]
public class CurPlayerMenu : ScriptableObject
{
    public List<MenuSlot> slots = new List<MenuSlot>();

    public bool HasMenu(Menu menu, out int index)
    {
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (menu == slots[i].Menu)
                {
                    index = i;
                    return true;
                }
            }
        }
        index = -1;
        return false;
    }

    public MenuSlot GetSlot(int index)
    {
        return slots[index];
    }

}

[Serializable]
public class MenuSlot
{
    public Menu Menu;
    public float completenessCount;

    public int GetCost()
    {
        int m = Menu.menu_Cost;
        float cpC = completenessCount;
        float cost = m * cpC;
        return (int)cost;
    }

}
