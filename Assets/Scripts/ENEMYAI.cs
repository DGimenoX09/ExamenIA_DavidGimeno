
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ENEMYAI : MonoBehaviour
{
    private NavMeshAgent _agent;

    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public EnemyState currentState;

    public Transform player; 
    public float detectionRange = 10f;
    public float attackRange = 2f; 
    public List<Transform> patrolPoints; 
    private int currentPatrolIndex = 0; 

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = EnemyState.Patrolling;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;

            case EnemyState.Chasing:
                Chase();
                break;

            case EnemyState.Attacking:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

       
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    void Chase()
    {
        
        _agent.SetDestination(player.position);

        
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = EnemyState.Attacking;
        }
       
        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = EnemyState.Patrolling;
            GoToNextPatrolPoint(); 
        }
    }

    void Attack()
    {
        
        Debug.Log("Navajazo al canto!");

       
        currentState = EnemyState.Chasing;
    }

    void GoToNextPatrolPoint()
    {
        
        if (patrolPoints.Count == 0) return;

       
        _agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }
}