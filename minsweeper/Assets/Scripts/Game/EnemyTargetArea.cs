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
        if (!Photon.Pun.PhotonNetwork.IsMasterClient)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()._nearEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Photon.Pun.PhotonNetwork.IsMasterClient || _thisEnemy._target == null)
            return;

        if (other.gameObject.CompareTag("Player")
            &&_thisEnemy._target.GetComponent<Photon.Pun.PhotonView>().ViewID == other.GetComponent<Photon.Pun.PhotonView>().ViewID)
        {
            // targetArea - target 놓침
            _thisEnemy._target = null;
            _thisEnemy.CancelTarget();
        }
    }
}
