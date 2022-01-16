using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] int _countALine;
    [SerializeField] int _startRoomNum;
    public List<Room> _roomList;
    public int _totalBomb;

    private void Start()
    {
        SetBomb();
    }

    private void SetBomb()
    {
        int count = 0;
        while (count < _totalBomb)
        {
            int i = Random.Range(0, 24);
            if (!_roomList[i]._isBomb &&
               (i < _startRoomNum - 1 || i > _startRoomNum + 1) &&
               i != _startRoomNum + _countALine && i != _startRoomNum - _countALine)
            {
                _roomList[i]._isBomb = true;
                count++;
            }
        }

        // Set _aroundBomb value
        for(int i = 0; i < _roomList.Count; i++)
        {
            if(_roomList[i]._roomType == RoomType.Center)
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
    }
}
