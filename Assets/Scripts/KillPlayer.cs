using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponentInParent<Player>();
        if (player)
            player.Damage(999999f, null);
    }
}
