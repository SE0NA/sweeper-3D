using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] Enemy _thisEnemy;

    float _sightAngle;
    float _sightDistance;
    [SerializeField] LayerMask _targetLayer = 0;

    void Start()
    {
        _sightAngle = _thisEnemy.GetEnemySightCP_sight_angle();
        _sightDistance = _thisEnemy.GetEnemySightCP_sight_distance();
    }

    void Update()
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            return;

        if (_thisEnemy._target == null)
        {
            Sight();
        }
    }

    private void Sight()
    {
        // �þ� ���� TargetLayer�� Object
        Collider[] objectsInSight = Physics.OverlapSphere(transform.position, _sightDistance, _targetLayer);

        if (objectsInSight.Length > 0)
        {
            Transform target = objectsInSight[0].transform;

            // Ÿ�� ���� ���
            Vector3 target_direction = (target.position - transform.position).normalized;
            float target_angle = Vector3.Angle(target_direction, transform.forward);
            if (target_angle < _sightAngle * 0.5f)
            {
                // Raycast �˻�
                if (Physics.Raycast(transform.position, target_direction, out RaycastHit target_hit, _sightDistance))
                {
                    _thisEnemy.SetTarget(target.transform);
                }
            }
        }
    }
}
