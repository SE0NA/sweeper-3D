using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Room _room1;
    [SerializeField] Room _room2;

    [SerializeField] GameObject leftdoor;
    [SerializeField] GameObject rightdoor; 


    public bool DoorOpen(int wherePlayer)
    {
        leftdoor.GetComponent<BoxCollider>().enabled = false;
        rightdoor.GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animation>().Play();   // �� ���� �ִϸ��̼�
        GetComponent<AudioSource>().Play(); // �� ���� ����
        GetComponent<BoxCollider>().enabled = false;    // ���̻� ��ȣ�ۿ� �Ұ�

        Room openedRoom;
        // �� ����
        if (wherePlayer == _room1.GetRoomNum())
            openedRoom=_room2;
        else
            openedRoom=_room1;

        openedRoom.RoomOpen();
        if (openedRoom._isBomb)
            return true;
        else
            return false;
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
