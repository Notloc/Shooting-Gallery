using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Equipment
{
    [Header("Required References")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzleTransform;

    [Header("Gun Options")]
    [SerializeField] float fireDelay = 10f;
    [SerializeField] float bulletDamage = 20f;
    [SerializeField] float bulletSpeed = 20f;
    [Space]
    [SerializeField] protected EquipmentSlotType slot;
    public override EquipmentSlotType SlotType { get { return slot; } }

    public override void PrimaryUse()
    {
        Shoot();
    }

    public override void SecondaryUse()
    {
        Aim();
    }

    protected virtual void Shoot()
    {
        Projectile pro = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.rotation);
        pro.Shoot(bulletSpeed, bulletDamage);
    }

    protected virtual void Aim()
    {

    }

}
