using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationEnemyState { Idle, Walk, Run, Died }
public class EnemyAnimationController : MonoBehaviour
{
    private Animator controller;
    private SpriteRenderer sprite;
    private State currentState;

    public bool Facing { get; private set; }

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //ChangeState(State.Idle);
    }

    public void ChangeState(State newState)
    {   
        SetBool(currentState, false);

        SetBool(newState, true);

        currentState = newState;
    }

    void SetBool(State state, bool value)
    {
        switch (state)
        {
            case State.Idle: 
                controller.SetBool("IsIdle", value);
                break;
            case State.Walk:
                controller.SetBool("IsWalk", value);
                break;
            case State.Run:
                controller.SetBool("IsRun", value);
                break;
            case State.Died:
                controller.SetBool("IsDied", value);
                break;
        }
    }

    public void SetFacing(float directionX)
    {
        if (directionX == 0) return;

        Facing = directionX < 0;

        sprite.flipX = Facing;
    }
}
