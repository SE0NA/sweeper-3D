using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: 타겟 추적 범위
    [SerializeField] Enemy _thisEnemy;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // targetArea - target 놓침
            _thisEnemy.CancelTarget();
        }
    }
}
