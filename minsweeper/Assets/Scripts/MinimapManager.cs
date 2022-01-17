using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    [SerializeField] Camera mapCam;
    [SerializeField] GameObject mapPointer;

    PlayerController _player;

    public void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        gameObject.transform.position.Set(_player.transform.position.x
                            , transform.position.y, _player.transform.position.z);
    }
}
