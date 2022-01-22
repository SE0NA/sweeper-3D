using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _navMeshAgent = null;
    public Transform _target = null;
    [SerializeField] int _defaultSpeed;
    [SerializeField] int _MaxSpeed;

    [SerializeField] Transform[] patrolPoints = null;
    int destinationPoint = 0;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _defaultSpeed;
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }

    void Update()
    {
        if (_target != null)
        {
            _navMeshAgent.SetDestination(_target.position);
        }
    }

    public void MoveToNextPoint()  // Patrol Points
    {
        if (_target == null)
        {
            if (_navMeshAgent.velocity == Vector3.zero)    // 정지 상태: 도착
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
                _navMeshAgent.SetDestination(patrolPoints[destinationPoint].position);
            }
        }
    }

    public void SetTarget(Transform thisTarget)     // <- EnemySight
    {
        CancelInvoke();
        _target = thisTarget;
        _navMeshAgent.speed = _MaxSpeed;
    }

    public void CancelTarget()  // <- EnenmyTargetArea
    {
        _target = null;
        _navMeshAgent.speed = _defaultSpeed;
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }
}
