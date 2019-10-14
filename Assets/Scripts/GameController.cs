using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Player Player { get; private set; }

    [Header("Required Reference")]
    [SerializeField] Player playerPrefab;
    [SerializeField] Transform spawnPoint;
    [Space]
    [SerializeField] BaseGun defaultGunPrefab;

    void Awake()
    {
        // Enforce singleton
        if (!Instance)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        Player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        if (defaultGunPrefab)
            Player.EquipmentManager.Equip(Instantiate(defaultGunPrefab));

        Cursor.lockState = CursorLockMode.Locked;
    }
}
