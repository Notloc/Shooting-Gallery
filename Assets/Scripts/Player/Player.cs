using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] new Camera camera;
    [SerializeField] EquipmentManager equipmentManager;

    [Header("Options")]
    [SerializeField] float interactionDistance = 2f;
    [SerializeField] LayerMask interactionLayerMask;


    public float Money { get; private set; } 
    public EquipmentManager EquipmentManager { get { return equipmentManager; } }

    void Update()
    {
        HandleInteraction();
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

}
