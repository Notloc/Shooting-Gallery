using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableGun : MonoBehaviour, IInteractable, IHaveInfo
{
    [Header("Required Reference")]
    [SerializeField] BaseGun gunPrefab;
    [Space]
    [SerializeField] float cost = 100f;

    public string GetInfoText()
    {
        return "Buy gun for $" + cost.ToString("#");
    }

    public void Interact(Player player)
    {
        if (player.SpendMoney(cost))
        {
            var gun = Instantiate(gunPrefab);
            player.EquipmentManager.Equip(gun, true);
        }
    }
}
