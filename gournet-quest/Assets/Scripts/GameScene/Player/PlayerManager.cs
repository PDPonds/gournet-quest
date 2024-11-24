using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBehavior
{
    Normal, UIShowing
}

public class PlayerManager : Singleton<PlayerManager>
{
    [HideInInspector] public PlayerInputSystem inputSystem;
    [HideInInspector] public PlayerUIManager uiManager;
    [HideInInspector] public CameraController cameraController;
    Rigidbody rb;
    Collider collider;

    PlayerBehavior curBehavior;

    [SerializeField] GameObject playerMesh;

    [Header("===== Mouse =====")]
    [HideInInspector] public Vector2 mousePos;
    [SerializeField] LayerMask mousePosMask;

    [Header("===== Controller =====")]
    [Header("- Move")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [Header("- Rotation")]
    [SerializeField] float rotationSpeed;
    float curMoveSpeed;
    Vector3 moveDir;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRun = false;

    [Header("===== Enegy =====")]
    [SerializeField] int maxEnergy;
    int curEnergy;

    [Header("===== Inventory =====")]
    public InventorySO player_Inventory;

    [Header("===== Hand Slot =====")]
    public Transform bulletSpawnPoint;
    [HideInInspector] public float curDelayTime;

    [Header("===== Interactive =====")]
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float interactiveLength;
    [HideInInspector] public IInteractable curInteractObj;

    public void SetupPlayer()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        SwitchBehavior(PlayerBehavior.Normal);
        ResetEnergy();

        uiManager = GetComponent<PlayerUIManager>();
        inputSystem = GetComponent<PlayerInputSystem>();

    }

    private void Update()
    {
        UpdateBehavior();
        HandleMoveSpeed();
        MoveHandle();
        RotationHandle();
        DecreaseDelayTime();
        InteractiveCheck();
    }

    #region Mouse
    public Vector3 GetWorldPosFormMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Vector3 worldPos = transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mousePosMask))
        {
            worldPos = hit.point;
        }

        return worldPos;
    }

    public Vector3 GetDirToMouse()
    {
        Vector3 mouseWorldPos = GetWorldPosFormMouse();
        Vector3 dir = mouseWorldPos - transform.position;
        dir.Normalize();
        dir.y = 0;
        return dir;
    }

    public Vector3 GetInvertDirFormMouse()
    {
        return GetDirToMouse() * -1f;
    }
    #endregion

    #region Controller
    void HandleMoveSpeed()
    {
        float overWeight = player_Inventory.GetWeightOver();
        if (isRun) curMoveSpeed = runSpeed - overWeight;
        else curMoveSpeed = walkSpeed - overWeight;
    }

    void MoveHandle()
    {
        if (CanMove())
        {
            moveDir = Camera.main.transform.forward * moveInput.y;
            moveDir = moveDir + Camera.main.transform.right * moveInput.x;
            moveDir.Normalize();
            moveDir.y = 0;
            moveDir = moveDir * curMoveSpeed;
        }
        else
        {
            moveDir = Vector3.zero;
        }

        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

    }

    void RotationHandle()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = Camera.main.transform.forward * moveInput.y;
        targetDir = targetDir + Camera.main.transform.right * moveInput.x;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRot;
        }
    }

    void LookAt(Vector3 targetDir)
    {
        if (targetDir == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        transform.rotation = targetRot;
    }

    bool CanMove()
    {
        return !isBehavior(PlayerBehavior.UIShowing);
    }
    #endregion

    #region Energy

    void ResetEnergy()
    {
        curEnergy = maxEnergy;
    }

    void AddEnergy(int amout)
    {
        curEnergy += amout;
        if (curEnergy > maxEnergy)
        {
            curEnergy = maxEnergy;
        }
    }

    bool RemoveEnergy(int amount)
    {
        if (HasEnergy(amount))
        {
            curEnergy -= amount;
            if (curEnergy <= 0)
            {
                curEnergy = 0;
            }
            return true;
        }

        return false;
    }

    bool HasEnergy(int amount)
    {
        return curEnergy >= amount;
    }

    #endregion

    #region Use Item In Hand
    public void UseItemInHandSlot()
    {
        if (isBehavior(PlayerBehavior.UIShowing)) return;
        if (curDelayTime > 0) return;
        if (uiManager.curHandSlotSelected == null) return;
        if (uiManager.curHandSlotSelected.transform.childCount < 1) return;

        Transform itemPrefab = uiManager.curHandSlotSelected.transform.GetChild(0);
        InventorySlotPrefab slotPrefab = itemPrefab.GetComponent<InventorySlotPrefab>();
        InventorySlot slot = player_Inventory.GetSlot(slotPrefab.slotIndex);
        ItemSO item = slot.Item;
        if (item is EquipmentItem equipmentItem)
        {
            equipmentItem.UseItem();
            LookAt(GetDirToMouse());
            if (player_Inventory.slots[slotPrefab.slotIndex].DecreaseDurabilityAndIsDestroy(equipmentItem.durabilityPerUse))
            {
                player_Inventory.ClearSlot(slotPrefab.slotIndex);
                uiManager.UpdateInventorySlot();
            }
            else
            {
                slotPrefab.UpdateDurability();
                uiManager.UpdateInventorySlot();
            }
            curDelayTime = equipmentItem.delayTime;
        }
    }

    void DecreaseDelayTime()
    {
        if (curDelayTime > 0)
        {
            curDelayTime -= Time.deltaTime;
            if (curDelayTime <= 0)
            {
                curDelayTime = 0;
            }
        }
    }
    #endregion

    #region Behavior
    public void SwitchBehavior(PlayerBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case PlayerBehavior.Normal:
                break;
            case PlayerBehavior.UIShowing:
                break;
        }
    }

    void UpdateBehavior()
    {
        switch (curBehavior)
        {
            case PlayerBehavior.Normal:
                break;
            case PlayerBehavior.UIShowing:
                break;
        }
    }

    public bool isBehavior(PlayerBehavior behavior)
    {
        return curBehavior == behavior;
    }

    #endregion

    void InteractiveCheck()
    {
        Collider[] interactiveObjs = Physics.OverlapSphere(transform.position + transform.forward * 0.5f, interactiveLength, interactiveMask);
        if (interactiveObjs.Length > 0)
        {
            if (interactiveObjs[0].TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                curInteractObj = interactable;
                uiManager.ShowInteractiveUI(curInteractObj.InteractInfo());
            }
            else
            {
                curInteractObj = null;
                uiManager.HideInteractiveUI();
            }
        }
        else
        {
            curInteractObj = null;
            uiManager.HideInteractiveUI();
        }
    }

    public void Interact()
    {
        if (isBehavior(PlayerBehavior.UIShowing)) return;
        if (curInteractObj == null) return;
        curInteractObj.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.5f, interactiveLength);
    }

}
