using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] GameObject impactParticlesPrefab;
    [SerializeField] SpriteRenderer bulletHolePrefab;

    public void Impact(Projectile projectile, RaycastHit hit)
    {
        Vector3 offset = hit.normal * 0.015f;

        // Create particles
        if (impactParticlesPrefab)
            Instantiate(impactParticlesPrefab, hit.point + offset, Quaternion.LookRotation(hit.normal));

        // Create bullet hole
        if (bulletHolePrefab)
        {
            SpriteRenderer newBulletHole = Instantiate(bulletHolePrefab, hit.point + offset, Quaternion.LookRotation(hit.normal));
            newBulletHole.sprite = projectile.BulletHole;
        }
    }
}
    