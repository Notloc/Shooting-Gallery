using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [Header("Required References")]
    [SerializeField] new Camera camera;
    [SerializeField] EquipmentManager equipmentManager;
    [SerializeField] PlayerController playerController;
    [SerializeField] GunManager gunController;
    [SerializeField] GameObject winScreenPrefab;
    [SerializeField] GameObject deathScreenPrefab;

    [Header("Options")]
    [SerializeField] float interactionDistance = 2f;
    [SerializeField] LayerMask interactionLayerMask;
    [Space]
    [SerializeField] float health = 100f;
    [SerializeField] float regenSpeed = 12f;
    [SerializeField] float regenDelay = 5f;


    public float Money { get; private set; } 
    public float Health { get { return health; } } 
    public EquipmentManager EquipmentManager { get { return equipmentManager; } }

    void Update()
    {
        HandleInteraction();
        RegenHealth();
    }

    private void HandleInteraction()
    {
        if (Input.GetButton(ControlBindings.FIRE_PRIMARY))
            equipmentManager.UsePrimary(this);

        if (Input.GetButton(ControlBindings.FIRE_SECONDARY))
            equipmentManager.UseSecondary(this);

        if (Input.GetButtonDown(ControlBindings.INTERACT))
            Interact();
    }

    private void RegenHealth()
    {
        if (lastDamage + regenDelay > Time.time)
            return;

        health += regenSpeed * Time.deltaTime;
        health = Mathf.Clamp(health, 0f, 100f);
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, interactionDistance, interactionLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
                interactable.Interact(this);
        }
    }

    public void AddMoney(float amount)
    {
        if (amount < 0f)
            return;

        Money += amount;
    }

    public bool SpendMoney(float amount)
    {
        if (amount > Money)
            return false;

        Money -= amount;
        return true;
    }

    float lastDamage = 0f;
    public void Damage(float amount, Player shooter)
    {
        if (shooter == this)
            return;

        lastDamage = Time.time; 
        health -= amount;
        if (health <= 0f)
            Die();
    }


    bool won, dead;

    public void Win()
    {
        if (won || dead)
            return;

        Instantiate(winScreenPrefab);

        playerController.enabled = false;
        equipmentManager.enabled = false;
        gunController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }

    private void Die()
    {
        if (won || dead)
            return;

        Instantiate(deathScreenPrefab);

        playerController.enabled = false;
        equipmentManager.enabled = false;
        gunController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }
}
