using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _thisEnemy = null;

    [SerializeField] Transform[] patrolPoints = null;
    int destinationPoint = 0;

    [SerializeField] float _sightAngle = 0f;
    [SerializeField] float _sightDistance = 0f;
    [SerializeField] LayerMask _targetLayer = 0;

    void Start()
    {
        _thisEnemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }

    void Update()
    {
        Sight();
    }

    void MoveToNextPoint()  // Patrol
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
            _thisEnemy.SetDestination(patrolPoints[destinationPoint].position);
        }
    }

    private void Sight()
    {
        // 시야 내의 오브젝트
        Collider[] objectsInSight = Physics.OverlapSphere(transform.position, _sightDistance, _targetLayer);

        if (objectsInSight.Length > 0)
        {
            Transform target = objectsInSight[0].transform;
            // target과의 각도 계산
            Vector3 target_direction = (target.position - transform.position).normalized;
            float target_angle = Vector3.Angle(target_direction, transform.forward);
            if (target_angle < _sightAngle * 0.5f)
            {
                // 장애물 검사
                if (Physics.Raycast(transform.forward, target_direction, out RaycastHit target_hit, _sightDistance))
                {
                    // what to do
                }
            }
        }
    }
}
