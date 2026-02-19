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
    [Tooltip("Scream")]
    public InputAction screamAction;

    [Header("Scream Settings")]
    [Tooltip("Scream cooldown")]
    public float screamCooldown = 2f;
    [Tooltip("Scream cooldown")]
    public GameObject screamPrefab;

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
        screamAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        runAction.Disable();
        screamAction.Disable();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if(!isScreaming)
            HandleMovementInput();

        HandleScream();
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

    bool isScreaming = false;

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(screamCooldown);
        isScreaming = false;
    }

    public void HandleScream()
    {
        if(isScreaming) return;

        if (screamAction.WasPressedThisFrame())
        {
            isScreaming = true;
            playerAnimation.ChangeState(State.Idle);
            rb.velocity = Vector2.zero;
            StartCoroutine(Cooldown());

            if(screamPrefab != null)
            {
                Instantiate(screamPrefab, transform.position, transform.rotation, null);
            }
        }
    }

}
