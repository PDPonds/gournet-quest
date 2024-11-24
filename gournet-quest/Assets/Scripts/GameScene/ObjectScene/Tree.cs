using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO product;
    [SerializeField] int minDropPerGet;
    [SerializeField] int maxDropPerGet;

    bool hasProduct;

    private void Start()
    {
        hasProduct = true;
    }

    public void Interact()
    {
        if (!hasProduct) return;

        int count = GetDropCount();
        PlayerManager.Instance.player_Inventory.AddItem(product, count);
        hasProduct = false;
    }

    int GetDropCount()
    {
        return Random.Range(minDropPerGet, maxDropPerGet);
    }

    public string InteractInfo()
    {
        return $"[E] to take {product.item_Name}";
    }
}
