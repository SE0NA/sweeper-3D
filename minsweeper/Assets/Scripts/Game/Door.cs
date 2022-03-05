using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isClose = true;
    
    [SerializeField] Room _room1;
    [SerializeField] Room _room2;

    [SerializeField] GameObject leftdoor;
    [SerializeField] GameObject rightdoor; 

    public void DoorOpen(int wherePlayer)
    {
        if (!isClose) return;
        GetComponent<Animation>().Play();   // 문 열림 애니메이션
        GetComponent<AudioSource>().Play(); // 문 열림 사운드
        GetComponent<BoxCollider>().enabled = false;    // 더이상 상호작용 불가
        isClose = false;

        if (wherePlayer == _room1.GetRoomNum())
            _room2.RoomOpen();
        else
            _room1.RoomOpen();
    }


    public void DoorFlag(int wherePlayer)
    {
        Room tmpRoom;
        if (wherePlayer == _room1.GetRoomNum())
            tmpRoom = _room2;
        else
            tmpRoom = _room1;

        if (!tmpRoom._isFlag)
            tmpRoom.RoomFlag();
        else
            tmpRoom.RoomUnFlag();
    }
}
