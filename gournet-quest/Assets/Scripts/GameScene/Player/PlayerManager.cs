using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerInputSystem inputSystem;

    public void SetupPlayer()
    {
        inputSystem = GetComponent<PlayerInputSystem>();
    }

}
