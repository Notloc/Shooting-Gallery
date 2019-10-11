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

    private void Update()
    {
        if (toggleAim)
        {
            if (Input.GetButtonDown(ControlBindings.AIM))
            {
                isAiming = !isAiming;
                foreach (var gun in activeGuns)
                    gun.SetADS(isAiming);
            }
        }
        else
        {
            bool newState = Input.GetButton(ControlBindings.AIM);
            if (isAiming != newState)
            {
                isAiming = newState;
                foreach (var gun in activeGuns)
                    gun.SetADS(isAiming);
            }
        }  

        if (Input.GetButtonDown(ControlBindings.RELOAD))
            foreach (var gun in activeGuns)
                gun.Reload();

        foreach (var gun in activeGuns)
            ProcessGun(gun);
    }

    private void ProcessGun(BaseGun gun)
    {
        gun.ReduceInaccuracy(Time.deltaTime);
        gun.Aim(aimingPov.position, aimingPov.forward);
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
