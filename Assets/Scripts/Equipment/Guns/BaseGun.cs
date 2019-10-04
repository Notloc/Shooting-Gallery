using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Equipment
{
    [Header("Required References")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzleTransform;

    [Header("Gun Options")]
    [SerializeField] protected EquipmentSlotType slotType;
    [Space]
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] float bulletDamage = 20f;
    [SerializeField] float bulletSpeed = 20f;
    [Space]
    [SerializeField] float aimSpeed = 5f;
    [SerializeField] [Range(0f, 2f)] float accuracyRecoveryRate = 0.5f;
    [SerializeField] float inaccuracyPerShot = 0.2f;
    [SerializeField] float inaccuracyStrength = 20f;
    [SerializeField] float spreadStrength = 5f;
    [SerializeField] AnimationCurve inaccuracyXCurve;
    [SerializeField] AnimationCurve inaccuracyYCurve;
    [SerializeField] AnimationCurve randomSpreadCurve;

    public override EquipmentSlotType SlotType { get { return slotType; } }
    private float shootTimer = 0f;
    private float inaccuracy = 0f;
    private float spreadX, spreadY;

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

            IncreaseInaccuracy(inaccuracyPerShot);
            UpdateSpread();
            ApplyInaccuracy();
        }
    }

    protected virtual void Aim()
    {

    }

    public void AimAt(Vector3 target)
    {
        Vector3 muzzleOffset = muzzleTransform.position - this.transform.position;
        target -= muzzleOffset;

        Quaternion newRot = Quaternion.LookRotation(target - this.transform.position);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, Time.deltaTime * aimSpeed);
    }

    private void ApplyInaccuracy()
    {
        float x = inaccuracyXCurve.Evaluate(inaccuracy);
        float y = inaccuracyYCurve.Evaluate(inaccuracy);

        x *= inaccuracyStrength;
        y *= inaccuracyStrength;

        x += spreadX;
        y += spreadY;

        this.transform.rotation *= Quaternion.Euler(-y, x, 0f);
    }

    public void IncreaseInaccuracy(float amount)
    {
        inaccuracy += amount;
        inaccuracy = Mathf.Clamp(inaccuracy, 0f, 1f);
    }
    public void ReduceInaccuracy(float deltaTime)
    {
        inaccuracy -= deltaTime * accuracyRecoveryRate;
        float spreadRecoveryMult = Mathf.Clamp((1f - (accuracyRecoveryRate * deltaTime)), 0f, 1f);

        spreadX *= spreadRecoveryMult;
        spreadY *= spreadRecoveryMult;
    }

    private void UpdateSpread()
    {
        float spreadMax = randomSpreadCurve.Evaluate(inaccuracy) * spreadStrength;
        spreadX = Random.Range(-spreadMax, spreadMax);
        spreadY = Random.Range(-spreadMax, spreadMax);
    }
}
