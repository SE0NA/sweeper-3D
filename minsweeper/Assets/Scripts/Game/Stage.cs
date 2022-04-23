using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Stage : MonoBehaviourPunCallbacks
{
    [SerializeField] int _countALine;
    public int _startRoomNum = 12;
    public List<Room> _roomList;
    public List<Door> _doorList;
    public int _totalBomb;

    Hashtable CP;

    private void Awake()
    {
        CP = PhotonNetwork.CurrentRoom.CustomProperties;
        _totalBomb = (int)CP["totalBomb"];
        SetBombByCP();
    }

    private void SetBombByCP()
    {
        // isbomb
        for (int i = 0; i < 25; i++)
            _roomList[i]._isBomb = (bool)CP["isBomb" + i.ToString()];
        // aroundBomb
        for (int i = 0; i < _roomList.Count; i++)
        {
            if (_roomList[i]._roomType == RoomType.Center)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.SideU)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.SideL)
            {
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.SideD)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.SideR)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.CornerUL)
            {
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.CornerUR)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i + _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.CornerDL)
            {
                if (_roomList[i + 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
            else if (_roomList[i]._roomType == RoomType.CornerDR)
            {
                if (_roomList[i - 1]._isBomb) _roomList[i]._aroundBomb++;
                if (_roomList[i - _countALine]._isBomb) _roomList[i]._aroundBomb++;
            }
        }
        Debug.Log("SetBomb ¿Ï·á!");
    }
}
