using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equipment/Ward")]
public class WardItem : EquipmentItem
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletTime;

    public WardItem()
    {
        equipment_Type = EquipmentType.Ward;
    }

    public override void UseItem()
    {
        base.UseItem();
        GameObject obj = Instantiate(bulletPrefab, PlayerManager.Instance.bulletSpawnPoint.position, Quaternion.identity);
        Bullet bullet = obj.GetComponent<Bullet>();
        Vector3 dir = PlayerManager.Instance.GetDirToMouse();
        bullet.Setup(dir, bulletSpeed, bulletTime);
    }

}
