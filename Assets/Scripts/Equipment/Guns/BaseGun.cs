using UnityEngine;

public class BaseGun : Equipment
{
    [Header("Gun Options")]
    [SerializeField] protected EquipmentSlotType slotType;
    [SerializeField] Vector3 holdPosition;

    [Header(" - Projectile")]
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] float bulletDamage = 20f;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] Vector3 kickBack;
    [SerializeField] float recenterSpeed = 15f;
    
    [Header(" - Aiming")]
    [SerializeField] float zeroingDistance = 100f;
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
    public override Vector3 HoldPosition { get { return holdPosition; } }

    private float shootTimer = 0f;
    private float inaccuracy = 0f;

    public override void PrimaryUse()
    {
        Shoot();
    }

    public override void SecondaryUse()
    {}

    public void Reload()
    {
        clip.DoClipEffect();
    }

    protected virtual void Shoot()
    {
        if (Time.time > shootTimer)
        {
            shootTimer = Time.time + fireDelay;

            Projectile pro = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation * CalculateSpread());
            pro.Shoot(bulletSpeed, bulletDamage);

            this.transform.localPosition += kickBack;

            IncreaseInaccuracy(inaccuracyPerShot);
            ApplyInaccuracy();
        }
    }

    private Quaternion CalculateSpread()
    {
        float spreadMax = randomSpreadCurve.Evaluate(inaccuracy) * randomSpreadStrength;
        float spreadX = Random.Range(-spreadMax, spreadMax);
        float spreadY = Random.Range(-spreadMax, spreadMax);

        if (isAiming)
        {
            spreadX /= aimingAccuracyStrength;
            spreadY /= aimingAccuracyStrength;
        }

        return Quaternion.Euler(spreadY, spreadX, 0f);
    }

    bool isAiming;
    Vector3 activeAimingOffset;
    public virtual void SetADS(bool state)
    {
        isAiming = state;
        if (isAiming)
            activeAimingOffset = aimingOffset;
        else
            activeAimingOffset = Vector3.zero;
    }



    /// <summary>
    /// Returns the direction the gun will be aiming the next time its fired
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAim()
    {
        Vector3 forward = muzzlePosition.forward;

        // Apply correction if shooting is on cooldown
        if (Time.time < shootTimer)
        {
            float timeRemaining = shootTimer - Time.time;

            Vector3 target = Camera.main.transform.position + 
                                Camera.main.transform.forward * zeroingDistance;
            
            Vector3 muzzleOffset = muzzlePosition.position - this.transform.position;
            target -= muzzleOffset;

            Quaternion newRot = Quaternion.LookRotation(target - this.transform.position);

            Quaternion currentRot = this.transform.rotation;
            while (0f < timeRemaining)
            {
                currentRot = Quaternion.Slerp(currentRot, newRot, Time.fixedDeltaTime * aimingSpeed);
                timeRemaining -= Time.fixedDeltaTime;
            }

            forward = (Quaternion.Inverse(this.transform.rotation) * currentRot) * forward;
        }

        RaycastHit hit;
        if (Physics.Raycast(muzzlePosition.position, forward, out hit, zeroingDistance))
            return hit.point;

        return muzzlePosition.position + (muzzlePosition.forward * zeroingDistance);
    }

    public void Aim(Vector3 povPosition, Vector3 direction)
    {
        Vector3 target = povPosition + (direction.normalized * zeroingDistance);

        Vector3 muzzleOffset = muzzlePosition.position - this.transform.position;
        target -= muzzleOffset;

        Quaternion newRot = Quaternion.LookRotation(target - this.transform.position);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, Time.fixedDeltaTime * aimingSpeed);
    }

    public void Recenter()
    {
        float dist = Mathf.Clamp(this.transform.localPosition.magnitude, 0.5f, 15f);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero + activeAimingOffset, Time.fixedDeltaTime * recenterSpeed * dist);
    }

    private void ApplyInaccuracy()
    {
        float x = inaccuracyXCurve.Evaluate(inaccuracy);
        float y = inaccuracyYCurve.Evaluate(inaccuracy);

        x *= inaccuracyStrength;
        y *= inaccuracyStrength;

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
