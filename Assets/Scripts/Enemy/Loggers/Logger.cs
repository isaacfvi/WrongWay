using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logger : EnemyController
{
    public GameObject weapon;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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
