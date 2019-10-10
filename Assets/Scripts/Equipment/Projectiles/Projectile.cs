using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] Sprite bulletHole;
    public Sprite BulletHole { get { return bulletHole; } }

    public abstract void Shoot(float velocity, float damage);
}
