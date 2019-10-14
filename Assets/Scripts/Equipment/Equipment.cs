using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour, IInteractable
{
    public abstract EquipmentSlotType SlotType { get; }
    public abstract Vector3 HoldPosition { get; }

    public void Interact(Player player)
    {
        player.EquipmentManager.Equip(this);
    }

    public abstract void PrimaryUse(Player player);
    public abstract void SecondaryUse(Player player);

}
