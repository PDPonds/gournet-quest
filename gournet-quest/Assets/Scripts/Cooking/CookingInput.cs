using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingInput : MonoBehaviour
{
    InputSystem inputSystem;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();

            inputSystem.CookingInput.SpaceAction.performed += i => CookingManager.Instance.SpaceAction();

        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}
