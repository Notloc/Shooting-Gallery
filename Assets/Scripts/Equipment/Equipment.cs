using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Hand,
    TwoHands
}

public abstract class Equipment : MonoBehaviour
{
    public abstract EquipmentSlot Slot { get; }

    public abstract void PrimaryUse();
    public abstract void SecondaryUse();

}
