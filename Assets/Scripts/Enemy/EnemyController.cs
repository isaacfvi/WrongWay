using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;

        animationController = GetComponentInChildren<EnemyAnimationController>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void Update()
    {   
        if(player != null)
        {
            HandleVision();
        }

        HandleMoviment();
    }

    #region Moviment
    protected Transform target;  
    NavMeshAgent agent;
    EnemyAnimationController animationController;
    
    void HandleMoviment()
    {
        HandlePathfinding();
        HandleAnimation();
    }

    protected virtual void HandleAnimation()
    {
        animationController.SetFacing(agent.velocity.x);
    }

    void HandlePathfinding()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;

            if (HasReachedDestination())
            {
                Stop();
            }
        }
        else
        {
            Stop();
        }
    }

    protected void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    bool HasReachedDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (agent == null) return;

        Gizmos.color = Color.red;
        var path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }
    }

    #endregion

    #region Vision

    [Header("Enemy Vision Settings")]
    [Tooltip("Distance to view")]
    public float viewDistance = 5f;
    [Tooltip("Angle to view")]
    public float viewAngle = 90f;
    [Tooltip("Player Trasnform")]
    public Transform player;
    [Tooltip("Lost Time")]
    public float lostTime = 2f;

    bool wasSeeingPlayer = false;
    float lostTimeCounter = 0;

    void HandleVision()
    {
        Vector2 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;
        bool seeingNow = false;

        if (distance <= viewDistance)
        {
            float angle = Vector2.Angle(
                animationController.Facing ? - transform.right : transform.right, 
                dirToPlayer);

            if (angle <= viewAngle / 2f)
            {
                seeingNow = true;
            }
        }

        if (seeingNow)
        {
            lostTimeCounter = 0f;

            if (!wasSeeingPlayer) {
                OnSeePlayer();
                wasSeeingPlayer = true;
            }
        }
        else
        {
            // Lost vision
            if (wasSeeingPlayer)
            {
                lostTimeCounter += Time.deltaTime;

                if (lostTimeCounter >= lostTime)
                {
                    OnLostPlayer();
                    wasSeeingPlayer = false;
                }
            }
        }
    }

    protected virtual void OnSeePlayer() {}
    protected virtual void OnLostPlayer() {}

    #endregion

}
