using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logger : EnemyController
{
    public GameObject weapon;

    public IdleState IdleState { get; set; }
    public FollowState FollowState { get; set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new IdleState(this);
        FollowState = new FollowState(this);

        stateMachine.Initialize(IdleState);
    }

}
