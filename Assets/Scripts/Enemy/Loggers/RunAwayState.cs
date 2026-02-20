using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : IEnemyState
{
    Logger enemy;

    public RunAwayState(Logger enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.Follow(Vector2.zero);
        enemy.SetSpeed(5);

        enemy.gameObject.AddComponent<TimeToDestroy>();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        
    }
}
