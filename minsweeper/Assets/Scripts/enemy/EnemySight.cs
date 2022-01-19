/*
 *   enemy object의 시야
 *    : 시야 내에 있는 플레이어 오브젝트 여부 탐색
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
        // 시야 내의 오브젝트
        Collider[] objectsInSight = Physics.OverlapSphere(transform.position, _sightDistance, _sightLayerMask);

        if (objectsInSight.Length > 0)
        {
            Transform target = objectsInSight[0].transform;
            // target과의 각도 계산
            Vector3 target_direction = (target.position - transform.position).normalized;
            float target_angle = Vector3.Angle(target_direction, transform.forward);
            if(target_angle < _sightAngle * 0.5f)
            {
                // 장애물 검사
                if (Physics.Raycast(transform.forward, target_direction, out RaycastHit target_hit, _sightDistance))
                    if (target_hit.transform.CompareTag("Player"))
                    {
                        transform.position = Vector3.Lerp(transform.position, target_hit.transform.position, 0.02f);
                    }
            }
        }
    }
}
