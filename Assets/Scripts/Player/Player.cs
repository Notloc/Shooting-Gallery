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

    void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (Input.GetButton(ControlBindings.FIRE_PRIMARY))
            equipmentManager.UsePrimary();

        if (Input.GetButton(ControlBindings.FIRE_SECONDARY))
            equipmentManager.UseSecondary();

        if (Input.GetButtonDown(ControlBindings.INTERACT))
            Interact();
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, interactionDistance))
        {
            Equipment equip = hit.collider.GetComponentInParent<Equipment>();
            if (equip)
                equipmentManager.Equip(equip);
        }
    }
}
