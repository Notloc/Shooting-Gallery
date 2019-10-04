using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] new Camera camera;

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

    }

    private void Rotate()
    {

    }

    private void Move()
    {

    }
}
