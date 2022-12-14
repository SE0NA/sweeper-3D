using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetArea : MonoBehaviour
{
    // TargetArea: 타겟 추적 범위
    // Player 텔레포트 기능 사용 불가

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
                // targetArea - target 놓침
                _thisEnemy._target = null;
                _thisEnemy.CancelTarget();
            }
        }
    }
}
