using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    public abstract EquipmentSlotType SlotType { get; }
    public abstract Vector3 HoldPosition { get; }


    public abstract void PrimaryUse();
    public abstract void SecondaryUse();

}
