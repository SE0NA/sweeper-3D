using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    [SerializeField] Enemy _thisEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _thisEnemy.KillPlayer();
            FindObjectOfType<GameManager>().GameOver();
        }
    }
}
