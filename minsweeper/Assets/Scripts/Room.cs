using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    CornerUL, CornerUR, CornerDL, CornerDR,
    SideU, SideL, SideD, SideR, Center
}

public class Room : MonoBehaviour
{
    [SerializeField] int _roomNum;
    public RoomType _roomType;

    public bool _isBomb = false;
    public bool _isOpened = false;
    public bool _isFlag = false;
    public int _aroundBomb = 0;

    [SerializeField] List<Light> _stateLights;

    public int GetRoomNum()
    {
        return _roomNum;
    }

    public void RoomOpen()
    {
        if (_isFlag)
            RoomUnFlag();

        if (_isBomb)
        {

        }
        else
        {
            _isOpened = true;
        }
    }
    public void RoomFlag()
    {
        _isFlag = true;

        StateLightOn();
    }
    public void RoomUnFlag()
    {
        _isFlag = false;
        StateLightOff();
    }

    void StateLightOn()
    {
        for (int i = 0; i < _stateLights.Count; i++)
            _stateLights[i].enabled = true;
    }
    void StateLightOff()
    {
        for (int i = 0; i < _stateLights.Count; i++)
            _stateLights[i].enabled = false;
    }
}
