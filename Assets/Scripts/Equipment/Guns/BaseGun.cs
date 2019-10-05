using UnityEngine;

public class BaseGun : Equipment
{
    [Header("Gun Options")]
    [SerializeField] protected EquipmentSlotType slotType;
    [Header(" - Projectile")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzleTransform;
    [SerializeField] float bulletDamage = 20f;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] Vector3 kickBack;
    [SerializeField] float recenterSpeed = 15f;
    
    [Header(" - Aiming")]
    [SerializeField] float aimingSpeed = 5f;
    [SerializeField] [Range(1f, 10f)] float aimingAccuracyStrength = 2;
    [SerializeField] Vector3 aimingOffset;
    [SerializeField] float randomSpreadStrength = 5f;
    [SerializeField] AnimationCurve randomSpreadCurve;
    [SerializeField] float inaccuracyPerShot = 0.2f;
    [SerializeField] [Range(0f, 2f)] float accuracyRecoveryRate = 0.5f;
    [SerializeField] float inaccuracyStrength = 20f;
    [SerializeField] AnimationCurve inaccuracyXCurve;
    [SerializeField] AnimationCurve inaccuracyYCurve;

    [Header(" - Clip")]
    [SerializeField] Clip clip;


    public override EquipmentSlotType SlotType { get { return slotType; } }
    private float shootTimer = 0f;
    private float inaccuracy = 0f;

    public override void PrimaryUse()
    {
        Shoot();
    }

    public override void SecondaryUse()
    {
        Aim();
    }

    public void Reload()
    {
        clip.DoClipEffect();
    }

    protected virtual void Shoot()
    {
        if (Time.time > shootTimer)
        {
            shootTimer = Time.time + fireDelay;

            Projectile pro = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.rotation);
            pro.Shoot(bulletSpeed, bulletDamage);

            this.transform.localPosition += kickBack;

            IncreaseInaccuracy(inaccuracyPerShot);
            ApplyInaccuracy();
        }
    }

    bool isAiming;
    Vector3 activeAimingOffset;
    protected virtual void Aim()
    {
        isAiming = !isAiming;
        if (isAiming)
            activeAimingOffset = aimingOffset;
        else
            activeAimingOffset = Vector3.zero;
    }

    public Vector3 GetAim()
    {
        Vector3 forward = muzzleTransform.forward;

        if (Time.time < shootTimer)
        {
            float dif = shootTimer - Time.time;

            Vector3 target = Camera.main.transform.position + 
                                Camera.main.transform.forward * 100f;
            

            Vector3 muzzleOffset = muzzleTransform.position - this.transform.position;
            target -= muzzleOffset;

            Quaternion newRot = Quaternion.LookRotation(target - this.transform.position);

            Quaternion currentRot = this.transform.rotation;
            while (Time.fixedDeltaTime < dif)
            {
                currentRot = Quaternion.Slerp(currentRot, newRot, Time.fixedDeltaTime * aimingSpeed);
                dif -= Time.fixedDeltaTime;
            }
            currentRot = Quaternion.Slerp(currentRot, newRot, dif * aimingSpeed);


            forward = (Quaternion.Inverse(this.transform.rotation) * currentRot) * forward;

        }

        RaycastHit hit;
        if (Physics.Raycast(muzzleTransform.position, forward, out hit, 100f))
            return hit.point;

        return muzzleTransform.position + (muzzleTransform.forward * 100f);
    }

    public void AimAt(Vector3 target)
    {
        Vector3 muzzleOffset = muzzleTransform.position - this.transform.position;
        target -= muzzleOffset;

        Quaternion newRot = Quaternion.LookRotation(target - this.transform.position);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, Time.deltaTime * aimingSpeed);
    }

    public void Recenter()
    {
        float dist = Mathf.Clamp(this.transform.localPosition.magnitude, 0.5f, 15f);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero + activeAimingOffset, Time.deltaTime * recenterSpeed * dist);
    }

    private void ApplyInaccuracy()
    {
        float x = inaccuracyXCurve.Evaluate(inaccuracy);
        float y = inaccuracyYCurve.Evaluate(inaccuracy);

        x *= inaccuracyStrength;
        y *= inaccuracyStrength;


        float spreadMax = randomSpreadCurve.Evaluate(inaccuracy) * randomSpreadStrength;
        float spreadX = Random.Range(-spreadMax, spreadMax);
        float spreadY = Random.Range(-spreadMax, spreadMax);

        x += spreadX;
        y += spreadY;

        if (isAiming)
        {
            x /= aimingAccuracyStrength;
            y /= aimingAccuracyStrength;
        }

        this.transform.rotation *= Quaternion.Euler(-y, x, 0f);
    }

    public void IncreaseInaccuracy(float amount)
    {
        inaccuracy += amount;
        inaccuracy = Mathf.Clamp(inaccuracy, 0f, 1f);
    }
    public void ReduceInaccuracy(float deltaTime)
    {
        if (shootTimer < Time.time)
        {
            inaccuracy -= deltaTime * accuracyRecoveryRate;
            float spreadRecoveryMult = Mathf.Clamp((1f - (accuracyRecoveryRate * deltaTime)), 0f, 1f);
        }
    }
}
