using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    InputSystem inputSystem;
    PlayerUIManager playerUIManager;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.PlayerInput.Moving.performed += i => PlayerManager.Instance.moveInput = i.ReadValue<Vector2>();
            inputSystem.PlayerInput.MousePosition.performed += i => PlayerManager.Instance.mousePos = i.ReadValue<Vector2>();
            inputSystem.PlayerInput.Run.performed += i => PlayerManager.Instance.isRun = true;
            inputSystem.PlayerInput.Run.canceled += i => PlayerManager.Instance.isRun = false;

            PlayerUIManager playerUIManager = transform.GetComponent<PlayerUIManager>();
            inputSystem.PlayerInput.ToggleInventory.canceled += i => playerUIManager.ToggleInventoryPanel();

            inputSystem.PlayerInput.SelectHandSlot_1.performed += i => playerUIManager.SelectHandSlot(0);
            inputSystem.PlayerInput.SelectHandSlot_2.performed += i => playerUIManager.SelectHandSlot(1);
            inputSystem.PlayerInput.SelectHandSlot_3.performed += i => playerUIManager.SelectHandSlot(2);
            inputSystem.PlayerInput.SelectHandSlot_4.performed += i => playerUIManager.SelectHandSlot(3);

            inputSystem.PlayerInput.UseItem.canceled += i => PlayerManager.Instance.UseItemInHandSlot();

            inputSystem.PlayerInput.Interact.canceled += i => PlayerManager.Instance.Interact();

        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}
