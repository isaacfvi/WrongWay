using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Moviment")]
    public InputAction moveAction;


    public float movementSpeed = 4.0f;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void  OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        HandleMovementInput();  
    }

    private void HandleMovementInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector2 movementForce = (transform.right * input.x + transform.up * input.y).normalized * movementSpeed;

        MovePlayer(movementForce);
    }

    private void MovePlayer(Vector2 movementForce)
    {
        rb.velocity = new Vector2(movementForce.x, movementForce.y);
    }

}
