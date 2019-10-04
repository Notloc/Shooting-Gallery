using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
    [Header("Required Reference")]
    [SerializeField] new Rigidbody rigidbody;

    [Header("Options")]
    [SerializeField] float gravity = 0f;
    [SerializeField] float airResistance = 0f;

    Vector3 velocity = Vector3.zero;
    float damage = 0f;
    public override void Shoot(float velocity, float damage)
    {
        this.velocity = this.transform.forward * velocity;
        this.damage = damage;
    }

    private void FixedUpdate()
    {
        // Update rotation
        this.transform.rotation = Quaternion.LookRotation(velocity);

        // Try to hit something
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, velocity, out hit, velocity.magnitude))
        {
            IDamagable damagable = hit.collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(damage);
            }
        }

        // Move and update velocity
        rigidbody.MovePosition(rigidbody.position + velocity);

        velocity.y += gravity;
        velocity *= (1f - airResistance);
    }
}
