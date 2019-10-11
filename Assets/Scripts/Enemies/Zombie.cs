using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Zombie : MonoBehaviour, IDamagable
{
    public NavMeshAgent Agent { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    [SerializeField] float fallSpeedRadians = 0.2f;

    private float health = 100f;
    public bool IsDead { get { return health <= 0f; } }

    private Transform target;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ProcessAi()
    {
        if (target)
            Agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Agent.enabled = false;
        Rigidbody.isKinematic = false;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        Rigidbody.angularVelocity += Rigidbody.rotation * new Vector3(fallSpeedRadians, 0f, fallSpeedRadians);

        this.gameObject.SetLayerRecursively(LayerManager.Effects);

        Destroy(this.gameObject, 15);
    }
}
