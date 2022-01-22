using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: 타겟 추적 범위
    // Player 맵 이동 불가

    [SerializeField] Enemy _thisEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()._nearEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // targetArea - target 놓침
            _thisEnemy._target = null;
            _thisEnemy.CancelTarget();
        }
    }
}
