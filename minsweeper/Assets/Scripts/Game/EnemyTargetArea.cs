using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: Ÿ�� ���� ����
    // Player �ڷ���Ʈ ��� ��� �Ұ�

    [SerializeField] Enemy _thisEnemy;

    void Start()
    {
        GetComponent<SphereCollider>().radius = _thisEnemy.GetEnemyTargetAreaCP_radius();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()._nearEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            return;

        if (other.gameObject.CompareTag("Player")){
            other.GetComponent<PlayerController>()._nearEnemy = false;
            if (_thisEnemy._target != null &&
                _thisEnemy._target.GetComponent<Photon.Pun.PhotonView>().ViewID
                == other.GetComponent<Photon.Pun.PhotonView>().ViewID)
            {
                // targetArea - target ��ħ
                _thisEnemy._target = null;
                _thisEnemy.CancelTarget();
            }
        }
    }
}
