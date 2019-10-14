using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGui : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Text healthText;

    void Update()
    {
        healthText.text = player.Health.ToString("#");
    }
}
