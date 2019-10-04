using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentManager))]
public class GunManager : MonoBehaviour
{
    [Header("Required Reference")]
    [SerializeField] new Camera camera;
    [Space]
    [SerializeField] float zeroingDistance;

    private HashSet<BaseGun> activeGuns;

    private EquipmentManager manager;
    private void Awake()
    {
        manager = GetComponent<EquipmentManager>();

        activeGuns = new HashSet<BaseGun>();

        manager.OnEquip += OnEquip;
        manager.OnUnequip += OnUnequip;
    }

    private void Update()
    {
        foreach (var gun in activeGuns)
            ProcessGun(gun);
    }

    private void ProcessGun(BaseGun gun)
    {
        gun.ReduceInaccuracy(Time.deltaTime);
        gun.AimAt(camera.transform.position + (camera.transform.forward * zeroingDistance));
    }

    private void OnEquip(Equipment equipment)
    {
        BaseGun gun = equipment as BaseGun;
        if (gun)
            RegisterGun(gun);
    }

    private void OnUnequip(Equipment equipment)
    {
        BaseGun gun = equipment as BaseGun;
        if (gun)
            UnregisterGun(gun);
    }

    private void RegisterGun(BaseGun gun)
    {
        if (activeGuns.Contains(gun) == false)
            activeGuns.Add(gun);
    }

    private void UnregisterGun(BaseGun gun)
    {
        if (activeGuns.Contains(gun))
            activeGuns.Remove(gun);
    }
}
