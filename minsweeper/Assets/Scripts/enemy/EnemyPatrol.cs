using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    NavMeshAgent _thisEnemy = null;
    bool _check = true;

    [SerializeField] Transform[] patrolPoints = null;
    int destinationPoint = 0;

    Animator enemyAnimator;

    void Start()
    {
        _thisEnemy = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        InvokeRepeating("MoveToNextPoint", 1f, 2f);
    }

    private void Update()
    {
        AnimCheck();
    }

    void MoveToNextPoint()
    {
        if (_thisEnemy.velocity == Vector3.zero)    // 정지 상태: 도착
        {
            while (true)
            {
                int next = Random.Range(0, patrolPoints.Length);
                if (next != destinationPoint)
                {
                    destinationPoint = next;
                    break;
                }
            }
            destinationPoint = Random.Range(0, patrolPoints.Length);
            _thisEnemy.SetDestination(patrolPoints[destinationPoint].position);
            enemyAnimator.Play("Move");
            _check = true;
        }
    }

    void AnimCheck()
    {
        if(_check && _thisEnemy.velocity == Vector3.zero)
        {
            _check = false;
            enemyAnimator.Play("Idle");
        }
    }
}
