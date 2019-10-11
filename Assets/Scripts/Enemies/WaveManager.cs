using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] ZombieManager zombieManager;

    [Header("Options")]
    [SerializeField] int baseWaveSize = 10;
    [SerializeField] float waveSizeFactor = 0.2f;

    private int wave = 0;

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
        wave++;

        int waveSize = Mathf.RoundToInt(baseWaveSize + (baseWaveSize * wave * waveSizeFactor));

        zombieManager.SpawnZombies(waveSize);
    }
}
