using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: Ÿ�� ���� ����
    [SerializeField] Enemy _thisEnemy;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // targetArea - target ��ħ
            _thisEnemy.CancelTarget();
        }
    }
}
