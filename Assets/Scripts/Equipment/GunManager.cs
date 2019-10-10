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
    public ICollection<BaseGun> ActiveGuns { get { return activeGuns; } }

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
        if (Input.GetButtonDown(ControlBindings.RELOAD))
            foreach (var gun in activeGuns)
                gun.Reload();

        foreach (var gun in activeGuns)
            ProcessGun(gun);
    }

    private void ProcessGun(BaseGun gun)
    {
        gun.ReduceInaccuracy(Time.deltaTime);
        gun.AimAt(camera.transform.position + (camera.transform.forward * zeroingDistance));
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
