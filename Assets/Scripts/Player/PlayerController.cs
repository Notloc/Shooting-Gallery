using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] new Camera camera;

    [Header("Options")]
    [SerializeField] float movementForce = 20f;
    [Space]
    [SerializeField] float xSensitivity = 3f;
    [SerializeField] float ySensitivity = 3f;
    [Space]
    [SerializeField] float minCameraPitch = -80f;
    [SerializeField] float maxCameraPitch = 80f;


    private float inputX, inputY, viewX, viewY;
    private float currentCameraRot = 0f;
    private void Update()
    {
        TakeInput();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void TakeInput()
    {
        inputX = Input.GetAxis(ControlBindings.MOVE_X);
        inputY = Input.GetAxis(ControlBindings.MOVE_Y);

        viewX = Input.GetAxis(ControlBindings.VIEW_X);
        viewY = Input.GetAxis(ControlBindings.VIEW_Y);
    }

    private void Rotate()
    {
        // Rotate player's body
        this.transform.rotation *= Quaternion.Euler(0f, viewX * xSensitivity, 0f);

        // Rotate camera
        currentCameraRot -= viewY * ySensitivity;
        currentCameraRot = Mathf.Clamp(currentCameraRot, minCameraPitch, maxCameraPitch);
        camera.transform.localRotation = Quaternion.Euler(currentCameraRot, 0f, 0f);
    }

    private void Move()
    {
        Vector3 force = new Vector3(inputX, 0f, inputY);
        if (force.sqrMagnitude > 1f)
            force = force.normalized;
        force *= movementForce;

        rigidbody.AddRelativeForce(force);
    }
}
