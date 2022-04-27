using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _navMeshAgent = null;
    public Transform _target = null;
    int _defaultSpeed;
    int _MaxSpeed;

    [SerializeField] Transform[] patrolPoints = null;
    int destinationPoint = 0;

    Animator enemyAnim;
    AudioSource enemyAudio;

    public bool _isEnd = false;
    public bool _isStart = false;

    void Start()
    {
        _defaultSpeed = (int)PhotonNetwork.CurrentRoom.CustomProperties["monster_defaultspeed"];
        _MaxSpeed = (int)PhotonNetwork.CurrentRoom.CustomProperties["monster_maxspeed"];

        if (!(bool)PhotonNetwork.CurrentRoom.CustomProperties["monster_sound"])
            GetComponent<AudioSource>().enabled = false;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _defaultSpeed;

        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();

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
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance )
            enemyAnim.SetBool("isMoving", false);
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
                enemyAnim.SetBool("isMoving", true);
            }
        }
    }

    public void SetTarget(Transform thisTarget)     // <- EnemySight
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        CancelInvoke();
        _target = thisTarget;
        enemyAnim.SetBool("isMoving", true);
        _navMeshAgent.stoppingDistance = 0;
        _navMeshAgent.speed = _MaxSpeed;
    }

    public void CancelTarget()  // <- EnenmyTargetArea
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        _target = null;
        _navMeshAgent.SetDestination(gameObject.transform.position);
        _navMeshAgent.speed = _defaultSpeed;
        _navMeshAgent.stoppingDistance = 0.5f;
        InvokeRepeating("MoveToNextPoint", 0f, 2f);
    }

    public void KillPlayer()
    {
        enemyAnim.SetTrigger("attack");
        CancelTarget();
    }

    public void PowerOffEnemy()
    {
        CancelInvoke();
        _navMeshAgent.SetDestination(gameObject.transform.position);
    }

    public void SetPatrolPointsFromGM(GameObject points)
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = points.transform.GetChild(i).transform;
        }
    }

    public float GetEnemySightCP_sight_angle() => (float)PhotonNetwork.CurrentRoom.CustomProperties["monster_sight_angle"];
    public float GetEnemySightCP_sight_distance() => (float)PhotonNetwork.CurrentRoom.CustomProperties["monster_sight_distance"];
    public float GetEnemyTargetAreaCP_radius() => (float)PhotonNetwork.CurrentRoom.CustomProperties["monster_targetarea_radius"];
}
