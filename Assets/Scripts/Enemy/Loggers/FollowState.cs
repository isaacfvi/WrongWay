using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IEnemyState
{
    Logger enemy;

    public FollowState(Logger enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.Follow(enemy.player);
    }
    public void Update()
    {
       if (!enemy.CanSeePlayer)
        {
            enemy.ChangeState(enemy.IdleState);
        } 
        
        float distance = Vector2.Distance(enemy.player.position, enemy.transform.position);

        if(distance <= enemy.attackRange)
        {
            enemy.ChangeState(enemy.AttackState);
        }
    }

        public void Exit()
    {
        
    }
}
