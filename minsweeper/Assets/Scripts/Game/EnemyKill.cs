using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    [SerializeField] Enemy _thisEnemy;
    [SerializeField] AudioClip clip_bite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().PlayOneShot(clip_bite);
            _thisEnemy.KillPlayer();

            other.GetComponent<PlayerController>().PlayerDie(isByMonster: true);
        }
    }
}

