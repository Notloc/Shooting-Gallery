using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] ZombieManager zombieManager;

    private void Start()
    {
        NextWave();
    }

    private void FixedUpdate()
    {
        if (zombieManager.AllZombiesDead)
            NextWave();
    }

    private void NextWave()
    {
        zombieManager.SpawnZombies(10);
    }
}
