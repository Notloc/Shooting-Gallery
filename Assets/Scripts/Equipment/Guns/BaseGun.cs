using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Equipment
{
    [Header("Required References")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzleTransform;

    [Header("Gun Options")]
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] float bulletDamage = 20f;
    [SerializeField] float bulletSpeed = 20f;
    [Space]
    [SerializeField] protected EquipmentSlotType slotType;
    public override EquipmentSlotType SlotType { get { return slotType; } }

    private float shootTimer = 0f;

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
        if (Time.time > shootTimer)
        {
            shootTimer = Time.time + fireDelay;

            Projectile pro = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.rotation);
            pro.Shoot(bulletSpeed, bulletDamage);
        }
    }

    protected virtual void Aim()
    {

    }

}
