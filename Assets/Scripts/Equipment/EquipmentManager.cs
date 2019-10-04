﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        equipment = new Dictionary<EquipmentSlot, Equipment>();

        equipmentSlots = new Dictionary<EquipmentSlot, Transform>();
        equipmentSlots.Add(EquipmentSlot.LeftHand, leftHandTransform);
        equipmentSlots.Add(EquipmentSlot.RightHand, rightHandTransform);
    }

    public bool Equip(Equipment newEquip)
    {
        EquipmentSlot slot = SlotTypeToOpenSlot(newEquip.SlotType);
        if (slot == EquipmentSlot.None)
            return false;

        newEquip.transform.SetParent(equipmentSlots[slot]);
        newEquip.transform.ResetLocal();
        equipment.Add(slot, newEquip);
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
            if (equipment.ContainsKey(EquipmentSlot.RightHand) || equipment.ContainsKey(EquipmentSlot.LeftHand))
                return EquipmentSlot.RightHand;        
        }
        return EquipmentSlot.None;
    }
}
