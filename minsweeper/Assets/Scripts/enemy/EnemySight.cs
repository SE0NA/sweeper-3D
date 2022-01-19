/*
 *   enemy object�� �þ�
 *    : �þ� ���� �ִ� �÷��̾� ������Ʈ ���� Ž��
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] float _sightAngle = 0f;
    [SerializeField] float _sightDistance = 0f;
    [SerializeField] LayerMask _sightLayerMask = 0;

    // Update is called once per frame
    void Update()
    {
        Sight();
    }

    private void Sight()
    {
        // �þ� ���� ������Ʈ
        Collider[] objectsInSight = Physics.OverlapSphere(transform.position, _sightDistance, _sightLayerMask);

        if (objectsInSight.Length > 0)
        {
            Transform target = objectsInSight[0].transform;
            // target���� ���� ���
            Vector3 target_direction = (target.position - transform.position).normalized;
            float target_angle = Vector3.Angle(target_direction, transform.forward);
            if(target_angle < _sightAngle * 0.5f)
            {
                // ��ֹ� �˻�
                if (Physics.Raycast(transform.forward, target_direction, out RaycastHit target_hit, _sightDistance))
                    if (target_hit.transform.CompareTag("Player"))
                    {
                        transform.position = Vector3.Lerp(transform.position, target_hit.transform.position, 0.02f);
                    }
            }
        }
    }
}
