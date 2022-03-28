using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    // Layer: Door ���X(�þ�)

    [SerializeField] Enemy _thisEnemy;

    [SerializeField] float _sightAngle = 0f;
    [SerializeField] float _sightDistance = 0f;
    [SerializeField] LayerMask _targetLayer = 0;

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
                Debug.Log("target 1");
                // Raycast �˻�
                if (Physics.Raycast(transform.position, target_direction, out RaycastHit target_hit, _sightDistance))
                {
                    Debug.Log("target 2");
                    _thisEnemy.SetTarget(target.transform);
                    Debug.Log("target 3");
                }
            }
        }
    }
}
