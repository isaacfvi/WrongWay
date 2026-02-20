using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logger : EnemyController
{
    public WeaponController weapon;
    public float attackRange = 1f;
    public float soundRange = 5f;

    public IdleState IdleState { get; set; }
    public FollowState FollowState { get; set; }
    public AttackState AttackState { get; set; }
    public RunAwayState RunAwayState { get; set; }

    protected override void Start()
    {
        base.Start();

        IdleState = new IdleState(this);
        FollowState = new FollowState(this);
        AttackState = new AttackState(this);
        RunAwayState = new RunAwayState(this);
        

        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        LookForScreams();
    }

    void LookForScreams()
    {
        GameObject[] screams = GameObject.FindGameObjectsWithTag("Scream");

        foreach (GameObject scream in screams)
        {
            float distanceSqr = (scream.transform.position - transform.position).sqrMagnitude;

            if (distanceSqr <= soundRange)
            {
                stateMachine.ChangeState(RunAwayState);
            }
        }
    }

}
