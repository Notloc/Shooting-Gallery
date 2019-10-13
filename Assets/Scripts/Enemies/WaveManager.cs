using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] ZombieManager zombieManager;
    [SerializeField] WaveGui waveGuiPrefab;

    [Header("Options")]
    [SerializeField] int baseWaveSize = 10;
    [SerializeField] float waveSizeFactor = 0.2f;
    [SerializeField] float intermissionLength = 45f;

    private int wave = 0;
    private WaveGui gui;

    private void Start()
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        gui = Instantiate(waveGuiPrefab, canvas.transform);

        NextWave();
    }

    int prevRemaining = 0;
    private void FixedUpdate()
    {
        if (prevRemaining != zombieManager.Remaining)
        {
            prevRemaining = zombieManager.Remaining;
            gui.UpdateRemaining(zombieManager.Remaining);
        }

        if (zombieManager.AllZombiesDead)
            NextWave();
    }

    bool waveStarting = false;
    private void NextWave()
    {
        if (waveStarting)
            return;

        wave++;
        StartCoroutine(StartWave(intermissionLength));
    }

    IEnumerator StartWave(float delay)
    {
        waveStarting = true;

        float waveStartTime = Time.time + delay;
        while(Time.time < waveStartTime)
        {
            gui.UpdateTimer(waveStartTime - Time.time);
            yield return null;
        }

        gui.UpdateTimer(0f);

        int waveSize = Mathf.RoundToInt(baseWaveSize + (baseWaveSize * (wave - 1) * waveSizeFactor));
        zombieManager.SpawnZombies(waveSize);
        waveStarting = false;
    }
}
