using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    [SerializeField] Camera mapCam;
    [SerializeField] GameObject mapPointer;

    PlayerController _player = null;
    Vector3 thisPos;

    public void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        thisPos = new Vector3(_player.transform.position.x
                            , gameObject.transform.position.y
                            , _player.transform.position.z);
        gameObject.transform.position = thisPos;
    }

    void Update()
    {
        if (_player != null)
        {
            thisPos = new Vector3(_player.transform.position.x
                                , gameObject.transform.position.y
                                , _player.transform.position.z);
            gameObject.transform.position = thisPos;
        }
    }
}
