using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    PlayerManager playerManager;


    public void SetupUIManager(PlayerManager manager)
    {
        playerManager = manager;
    }
}
