using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logger : EnemyController
{
    public WeaponController weapon;
    public float attackRange = 1f;

    public IdleState IdleState { get; set; }
    public FollowState FollowState { get; set; }

    public AttackState AttackState { get; set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new IdleState(this);
        FollowState = new FollowState(this);
        AttackState = new AttackState(this);

        stateMachine.Initialize(IdleState);
    }

}
