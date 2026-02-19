using UnityEngine;

public class AttackState : IEnemyState
{
    Logger enemy;

    float attackDuration = 0.5f;
    float timer;

    public AttackState(Logger enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.StopFollowing();
        enemy.weapon.Attack();

        timer = attackDuration;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.ChangeState(new FollowState(enemy));
            timer = attackDuration;
        }
    }

    public void Exit()
    {
        
    }
}
