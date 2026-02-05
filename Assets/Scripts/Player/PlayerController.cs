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
    private PlayerAnimationController playerAnimation;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<PlayerAnimationController>();
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

        Vector2 input = moveAction.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            if (runAction.IsPressed())
            {
                speed *= runScale;
                playerAnimation.ChangeState(State.Run);
            }
            else
            {
                playerAnimation.ChangeState(State.Walk);
            }

            playerAnimation.SetFacing(input.x);
            rb.velocity = input.normalized * speed;
        }
        else
        {
            playerAnimation.ChangeState(State.Idle);
            rb.velocity = Vector2.zero;
        }
    }

}
