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

    public void UpdateMenuSlot(Menu menu, float completenessCount)
    {
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].Menu == menu && slots[i].completenessCount < completenessCount)
                {
                    slots[i].completenessCount = completenessCount;
                    return;
                }
            }

            MenuSlot slot = new MenuSlot();
            slot.Menu = menu;
            slot.completenessCount = completenessCount;
            slots.Add(slot);
            return;

        }
        else
        {
            MenuSlot slot = new MenuSlot();
            slot.Menu = menu;
            slot.completenessCount = completenessCount;
            slots.Add(slot);
            return;
        }
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
        float cpC = completenessCount / 100f;
        float cost = m * cpC;
        return (int)cost;
    }

}
