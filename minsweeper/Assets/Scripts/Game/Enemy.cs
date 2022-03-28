using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _navMeshAgent = null;
    public Transform _target = null;
    [SerializeField] int _defaultSpeed;
    [SerializeField] int _MaxSpeed;

    [SerializeField] Transform[] patrolPoints = null;
    int destinationPoint = 0;

    Animator enemyAnim;

    public bool _isEnd = false;
    public bool _isStart = false;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _defaultSpeed;

        enemyAnim = GetComponent<Animator>();

        if (PhotonNetwork.IsMasterClient)
            Invoke("EnemyStart", 5f);
        _isStart = true;
    }
    private void EnemyStart()
    {
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (_target != null && _isStart)
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
                    int next = Random.Range(0, patrolPoints.Length - 1);
                    if (next != destinationPoint)
                    {
                        destinationPoint = next;
                        break;
                    }
                }
                _navMeshAgent.SetDestination(patrolPoints[destinationPoint].position);
                Debug.Log("NextPoint: " + (destinationPoint + 1));
                enemyAnim.Play("Run");
            }
        }
    }

    public void SetTarget(Transform thisTarget)     // <- EnemySight
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        CancelInvoke();
        _target = thisTarget;
        _navMeshAgent.speed = _MaxSpeed;
    }

    public void CancelTarget()  // <- EnenmyTargetArea
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        _target = null;
        _navMeshAgent.speed = _defaultSpeed;
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }

    public void KillPlayer()
    {
        enemyAnim.Play("Attack");
        CancelTarget();
    }

    public void PowerOffEnemy()
    {
        CancelInvoke();
    }

    public void SetPatrolPointsFromGM(GameObject points)
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = points.transform.GetChild(i).transform;
        }
    }
}
