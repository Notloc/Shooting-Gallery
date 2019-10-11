using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EquipmentSlot
{
    None,
    LeftHand,
    RightHand
}

public enum EquipmentSlotType
{
    OneHand,
    TwoHand
}

public class EquipmentManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform leftHandTransform;
    [SerializeField] Transform rightHandTransform;

    Dictionary<EquipmentSlot, Equipment> equipment;
    Dictionary<EquipmentSlot, Transform> equipmentSlots;

    public UnityAction<Equipment> OnEquip;
    public UnityAction<Equipment> OnUnequip;

    private void Awake()
    {
        equipment = new Dictionary<EquipmentSlot, Equipment>();

        equipmentSlots = new Dictionary<EquipmentSlot, Transform>();
        equipmentSlots.Add(EquipmentSlot.LeftHand, leftHandTransform);
        equipmentSlots.Add(EquipmentSlot.RightHand, rightHandTransform);
    }

    public bool Equip(Equipment newEquip)
    {
        EquipmentSlot slotId = SlotTypeToOpenSlot(newEquip.SlotType);
        if (slotId == EquipmentSlot.None)
            return false;

        Transform slot = equipmentSlots[slotId];
        slot.localPosition = newEquip.HoldPosition;

        newEquip.transform.SetParent(slot);
        newEquip.transform.ResetLocal();

        equipment.Add(slotId, newEquip);
        OnEquip?.Invoke(newEquip);
        return true;
    }


    public void UsePrimary()
    {
        EquipmentSlot slot = EquipmentSlot.RightHand;
        if (equipment.ContainsKey(slot))
            equipment[slot].PrimaryUse();
        else
        {
            slot = EquipmentSlot.LeftHand;
            if (equipment.ContainsKey(slot))
                equipment[slot].SecondaryUse();
        }
    }

    public void UseSecondary()
    {
        EquipmentSlot slot = EquipmentSlot.LeftHand;
        if (equipment.ContainsKey(slot))
            equipment[slot].PrimaryUse();
        else
        {
            slot = EquipmentSlot.RightHand;
            if (equipment.ContainsKey(slot))
                equipment[slot].SecondaryUse();
        }
    }

    /// <summary>
    /// Convert the given SlotType into an available equipment slot if possible
    /// </summary>
    /// <param name="slotType"></param>
    /// <returns></returns>
    private EquipmentSlot SlotTypeToOpenSlot(EquipmentSlotType slotType)
    {
        if (slotType == EquipmentSlotType.OneHand)
        {
            if (equipment.ContainsKey(EquipmentSlot.RightHand) == false)
                return EquipmentSlot.RightHand;

            else if (equipment.ContainsKey(EquipmentSlot.LeftHand) == false)
                return EquipmentSlot.LeftHand;
        }
        else
        {
            if (!equipment.ContainsKey(EquipmentSlot.RightHand) && !equipment.ContainsKey(EquipmentSlot.LeftHand))
                return EquipmentSlot.RightHand;        
        }
        return EquipmentSlot.None;
    }
}
