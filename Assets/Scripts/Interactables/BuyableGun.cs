using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableGun : MonoBehaviour, IInteractable
{
    [Header("Required Reference")]
    [SerializeField] BaseGun gunPrefab;
    [Space]
    [SerializeField] float cost = 100f;

    public void Interact(Player player)
    {
        if (player.SpendMoney(cost))
        {
            var gun = Instantiate(gunPrefab);
            player.EquipmentManager.Equip(gun, true);
        }
    }
}
