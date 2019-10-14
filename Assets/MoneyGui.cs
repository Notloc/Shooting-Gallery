using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGui : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Player player;
    [SerializeField] Text moneyText;

    void FixedUpdate()
    {
        moneyText.text = "$" + player.Money.ToString("#.##");
    }
}
