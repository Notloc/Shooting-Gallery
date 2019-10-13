using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Zombie zombiePrefab;
    [SerializeField] Transform[] spawnPoints;

    [Header("Options")]
    [SerializeField] int maxZombies = 15;
    [SerializeField] float spawnDelay = 2f;

    public bool AllZombiesDead { get { return zombiesToSpawn == 0 && activeZombies.Count == 0; } }
    public int Remaining { get { return activeZombies.Count + zombiesToSpawn; } }

    private Player player;
    private int zombiesToSpawn = 0;
    private float spawnTimer = 0f;
    private List<Zombie> activeZombies = new List<Zombie>();

    private void Awake()
    {
        player = GameController.Instance.Player;
    }

    public void SpawnZombies(int amount)
    {
        zombiesToSpawn += amount;
    }

    private void FixedUpdate()
    {
        TrySpawnZombie();
        RemoveDead();
        ProcessZombieAi();
    }

    private void TrySpawnZombie()
    {
        if (zombiesToSpawn <= 0 || spawnTimer > Time.time || activeZombies.Count >= maxZombies)
            return;

        Transform spawnPoint = ChooseSpawnPoint();
        Zombie newZomb = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        zombiesToSpawn--;

        newZomb.SetTarget(player.transform);
        activeZombies.Add(newZomb);

        spawnTimer = Time.time + spawnDelay;
    }

    private void RemoveDead()
    {
        for (int i=0; i<activeZombies.Count; i++)
        {
            if(activeZombies[i].IsDead)
            {
                activeZombies.RemoveAt(i);
                i--;
            }
        }
    }

    private void ProcessZombieAi()
    {
        foreach (var zombie in activeZombies)
        {
            zombie.ProcessAi();
        }
    }

    private Transform ChooseSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }
}
