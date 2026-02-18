using UnityEngine;

public class IdleState : IEnemyState
{
    Logger enemy;

    Vector2 patrolCenter;
    float patrolLength = 5;

    float waitTime = 5;

    float currentTime = 0;

    public IdleState(Logger enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.StopFollowing();
        patrolCenter = enemy.transform.position;
        currentTime = 0;
    }

    public void Update()
    {
        bool readyToMove = CheckTime();

        if (readyToMove)
        {
            Vector2 newPosition = GenerateNewPosition();
            enemy.Follow(newPosition);
        }

        if (enemy.CanSeePlayer)
        {
            enemy.ChangeState(enemy.FollowState);
        }
    }

    bool CheckTime()
    {
        if(currentTime < waitTime)
        {
            currentTime += Time.deltaTime;
            return false;
        }
        else
        {
            currentTime = 0;
            return true;
        }

    }

    private Vector2 GenerateNewPosition()
    {   
        Vector2 randomCircle = Random.insideUnitCircle * patrolLength;

        Vector2 patrolPoint = patrolCenter + new Vector2(randomCircle.x, randomCircle.y);

        return patrolPoint;
    }

    public void Exit()
    {
        
    }
}
