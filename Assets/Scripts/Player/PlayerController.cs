using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] new Camera camera;
    [SerializeField] Transform hands;

    [Header("Movement Options")]
    [SerializeField] float movementForce = 20f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float sprintMultiplier = 1.75f;
    [SerializeField] float velocityDecayMultiplier = 0.9f;
    [Space]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float airControlMultiplier = 0.15f;
    
    [Header("Aiming Options")]
    [SerializeField] float xSensitivity = 3f;
    [SerializeField] float ySensitivity = 3f;
    [Space]
    [SerializeField] float minCameraPitch = -80f;
    [SerializeField] float maxCameraPitch = 80f;

    [Header("Hand Sway Options")]
    [SerializeField] float maxHandRotation = 7f;
    [SerializeField] float handRotationSpeed = 4f;

    [Header("Ground Raycast Options")]
    [SerializeField] Vector3 groundCastOffset;
    [SerializeField] float groundCastLength = 0.03f;
    [SerializeField] [Range(4,12)] int numberOfGroundCasts = 4;
    [SerializeField] [Range(0.01f, 0.5f)] float groundCastRadius = 0.1f;
    [SerializeField] LayerMask groundCastLayerMask;

    private float inputX, inputY, viewX, viewY;
    private bool hasMovementInput, isSprinting, jumped;


    private bool isGrounded = false;
    private float currentCameraRot = 0f;
    private void Update()
    {
        TakeInput();
        Rotate();
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        Move();
    }

    private void TakeInput()
    {
        inputX = Input.GetAxis(ControlBindings.MOVE_X);
        inputY = Input.GetAxis(ControlBindings.MOVE_Y);

        viewX = Input.GetAxis(ControlBindings.VIEW_X);
        viewY = Input.GetAxis(ControlBindings.VIEW_Y);

        hasMovementInput = inputX != 0 || inputY != 0;
        isSprinting = Input.GetButton(ControlBindings.SPRINT);
        jumped = jumped || Input.GetButtonDown(ControlBindings.JUMP);
    }

    private void UpdateGrounded()
    {
        float angleStep = 360f / (float)numberOfGroundCasts;

#if UNITY_EDITOR
        // Show rays in the editor
        for (int i = 0; i < numberOfGroundCasts; i++)
        {
            Vector3 circluarOffset = Vector3.forward * groundCastRadius;
            circluarOffset = Quaternion.Euler(0f, i * angleStep, 0f) * circluarOffset;
            Debug.DrawRay(this.transform.position + circluarOffset + groundCastOffset, Vector3.down * groundCastLength, Color.green);
        }
#endif

        for (int i=0; i<numberOfGroundCasts; i++)
        {
            Vector3 circluarOffset = Vector3.forward * groundCastRadius;
            circluarOffset = Quaternion.Euler(0f, i * angleStep, 0f) * circluarOffset;

            if (Physics.Raycast(this.transform.position + circluarOffset + groundCastOffset, Vector3.down, groundCastLength, groundCastLayerMask))
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }

    Quaternion targetHandRotation = Quaternion.identity;
    private void Rotate()
    {
        // Rotate player's body
        this.transform.rotation *= Quaternion.Euler(0f, viewX * xSensitivity, 0f);

        // Rotate camera
        currentCameraRot -= viewY * ySensitivity;
        currentCameraRot = Mathf.Clamp(currentCameraRot, minCameraPitch, maxCameraPitch);
        camera.transform.localRotation = Quaternion.Euler(currentCameraRot, 0f, 0f);

        // Rotate hands
        hands.localRotation = Quaternion.Slerp(hands.localRotation, targetHandRotation, Time.deltaTime * handRotationSpeed);
    }

    private void Move()
    {
        if (isGrounded)
        {
            if (jumped)
            {
                jumped = false;

                rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }

        if (hasMovementInput)
        {
            Vector3 force = new Vector3(inputX, 0f, inputY);
            if (force.sqrMagnitude > 1f)
                force = force.normalized;

            force *= movementForce;
            if (isSprinting)
                force *= sprintMultiplier;

            if (!isGrounded)
                force *= airControlMultiplier;

            rigidbody.AddRelativeForce(force);
            rigidbody.velocity = ClampPlayerVelocity(rigidbody.velocity);
        }
        else
        { // Lose velocity quickly
            float oldY = rigidbody.velocity.y; // Preserve gravity;

            Vector3 newVelocity = rigidbody.velocity * velocityDecayMultiplier;
            newVelocity.y = oldY;

            rigidbody.velocity = newVelocity;
        }
        

        //Update target hand rotation
        Vector3 localVelocity = Quaternion.Inverse(rigidbody.rotation) * rigidbody.velocity;

        float x, y;
        x = Mathf.Clamp(localVelocity.x,-maxHandRotation, maxHandRotation);
        y = Mathf.Clamp(localVelocity.y, -maxHandRotation, maxHandRotation);

        targetHandRotation = Quaternion.Euler(y, -x, 0f);
    }

    /// <summary>
    /// Clamps the players velocity, excluding the y axis
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Vector3 ClampPlayerVelocity(Vector3 input)
    {
        float oldY = input.y;
        input.y = 0;

        float max = movementSpeed;
        if (isSprinting)
            max *= sprintMultiplier;

        input = Vector3.ClampMagnitude(input, max);
        input.y = oldY;

        return input;
    }
}
