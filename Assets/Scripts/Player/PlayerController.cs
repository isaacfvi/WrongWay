using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [Tooltip("Moviment")]
    public InputAction moveAction;
    [Tooltip("Run")]
    public InputAction runAction;

    [Header("Moviment Speed Settings")]
    public float movementSpeed = 4.0f;
    public float runScale = 1.5f;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void  OnEnable()
    {
        moveAction.Enable();
        runAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        runAction.Enable();
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
        float speed = movementSpeed;

        if (runAction.IsPressed())
        {
            speed *= runScale;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();

        rb.velocity = input.normalized * speed;
    }

}
