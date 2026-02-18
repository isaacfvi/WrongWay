using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logger : EnemyController
{
    public GameObject weapon;

    IdleState Idle { get; set; }

    protected override void Start()
    {
        base.Start();

        Idle = new IdleState(this);

        stateMachine.Initialize(Idle);
    }

    protected override void OnSeePlayer()
    {
        this.Follow(player);
    }

    protected override void OnLostPlayer()
    {
        this.StopFollowing();
    }
}
