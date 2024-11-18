using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    InputSystem inputSystem;
    PlayerManager playerManager;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.PlayerInput.Moving.performed += i => playerManager.moveInput = i.ReadValue<Vector2>();
            inputSystem.PlayerInput.MousePosition.performed += i => playerManager.mousePos = i.ReadValue<Vector2>();
            inputSystem.PlayerInput.Run.performed += i => playerManager.isRun = true;
            inputSystem.PlayerInput.Run.canceled += i => playerManager.isRun = false;
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    public void SetupInputSystem(PlayerManager manager)
    {
        playerManager = manager;
    }

}
