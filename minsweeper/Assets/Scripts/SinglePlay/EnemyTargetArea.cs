using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: Ÿ�� ���� ����
    // Player �� �̵� �Ұ�

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
            // targetArea - target ��ħ
            _thisEnemy._target = null;
            _thisEnemy.CancelTarget();
        }
    }
}
