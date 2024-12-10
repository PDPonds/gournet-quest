using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    MoveToOutSide, MoveToInSide
}

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] string room_Name;
    [SerializeField] Transform room_EnterPosition;
    [SerializeField] DoorType doorType;

    [SerializeField] bool isRestaurantDoor;

    public void Interact()
    {
        switch (doorType)
        {
            case DoorType.MoveToOutSide:
                PlayerManager.Instance.cameraController.SetupOutSide();
                if (isRestaurantDoor)
                {
                    PlayerManager.Instance.uiManager.open_and_close_restaurant_but.gameObject.SetActive(false);
                }
                break;
            case DoorType.MoveToInSide:
                PlayerManager.Instance.cameraController.SetupInSide();
                if (isRestaurantDoor)
                {
                    PlayerManager.Instance.uiManager.open_and_close_restaurant_but.gameObject.SetActive(true);
                }
                break;
        }

        PlayerManager.Instance.transform.position = room_EnterPosition.position;

        

    }

    public string InteractInfo()
    {
        return $"[E] to Enter the {room_Name}";
    }
}
