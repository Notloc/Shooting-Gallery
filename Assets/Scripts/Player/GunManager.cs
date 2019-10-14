using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentManager))]
public class GunManager : MonoBehaviour
{
    [Header("Required Reference")]
    [SerializeField] Transform aimingPov;

    [Header("Options")]
    [SerializeField] bool toggleAim;

    private HashSet<BaseGun> activeGuns;
    public ICollection<BaseGun> ActiveGuns { get { return activeGuns; } }

    private bool isAiming = false;

    private EquipmentManager equipmentManager;
    private void Awake()
    {
        equipmentManager = GetComponent<EquipmentManager>();

        activeGuns = new HashSet<BaseGun>();

        equipmentManager.OnEquip += RegisterGun;
        equipmentManager.OnUnequip += UnregisterGun;
    }

    bool aimInput, aimHold, reload;
    private void Update()
    {
        // Take Input

        aimInput = aimInput || Input.GetButtonDown(ControlBindings.AIM);
        aimHold = Input.GetButton(ControlBindings.AIM);

        reload = reload || Input.GetButtonDown(ControlBindings.RELOAD);
    }

    private void FixedUpdate()
    {
        if (toggleAim)
        {
            if (aimInput)
            {
                isAiming = !isAiming;
                foreach (var gun in activeGuns)
                    gun.SetADS(isAiming);
            }
            aimInput = false;
        }
        else
        {
            bool newState = aimHold;
            if (isAiming != newState)
            {
                isAiming = newState;
                foreach (var gun in activeGuns)
                    gun.SetADS(isAiming);
            }
        }  

        if (reload)
            foreach (var gun in activeGuns)
                gun.Reload();
        reload = false;

        foreach (var gun in activeGuns)
            ProcessGun(gun);
    }

    private void ProcessGun(BaseGun gun)
    {
        gun.ReduceInaccuracy(Time.fixedDeltaTime);
        gun.AimGun(aimingPov.position, aimingPov.forward);
        gun.Recenter();
    }

    private void RegisterGun(Equipment equip)
    {
        BaseGun gun = equip as BaseGun;
        if (gun && !activeGuns.Contains(gun))
            activeGuns.Add(gun);
    }

    private void UnregisterGun(Equipment equip)
    {
        BaseGun gun = equip as BaseGun;
        if (gun && activeGuns.Contains(gun))
            activeGuns.Remove(gun);
    }
}
