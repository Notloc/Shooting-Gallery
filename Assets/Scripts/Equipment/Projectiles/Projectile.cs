using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] protected Sprite bulletHole;
    [SerializeField] protected LayerMask collisionMask;

    public Sprite BulletHole { get { return bulletHole; } }
    public abstract void Shoot(Player shooter, float velocity, float damage);
}
