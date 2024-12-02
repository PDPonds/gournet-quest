using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        PlayerManager.Instance.uiManager.ShowCookingStation();
    }

    public string InteractInfo()
    {
        return $"[E] to Research.";
    }

}
