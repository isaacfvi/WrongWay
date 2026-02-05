using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator controller;
    private SpriteRenderer sprite;
    public enum State { Idle }
    private State currentState;

    void Awake()
    {
        controller = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        ChangeState(State.Idle);
    }

    public void ChangeState(State newState)
    {   
        SetBool(currentState, false);

        SetBool(newState, true);
    }

    void SetBool(State state, bool value)
    {
        switch (state)
        {
            case State.Idle: 
                controller.SetBool("IsIdle", value);
                break;
        }
    }

    public void SetFacing(float directionX)
    {
        if (directionX == 0) return;

        sprite.flipX = directionX < 0;
    }

}
