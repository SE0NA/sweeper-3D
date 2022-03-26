using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Stage : MonoBehaviourPunCallbacks
{
    [SerializeField] int _countALine;
    public int _startRoomNum;
    public List<Room> _roomList;
    public List<Door> _doorList;
    public int _totalBomb;

    public int _howManyRoomsForEnemy;

    PhotonView PV;

    int[] array_isbomb;
    int[] array_aroundBomb;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            array_isbomb = new int[25];
            array_aroundBomb = new int[25];

            SetBomb();

            PV.RPC("RPC_Sync_isBomb", RpcTarget.Others, array_isbomb, _totalBomb);
            PV.RPC("RPC_Sync_aroundBomb", RpcTarget.Others, array_aroundBomb);
            Debug.Log("Stage 동기화 완료");
        }
    }

    private void SetBomb()
    {
        int count = 0;
        int i_isbomb = 0;

        while (count < _totalBomb)
        {
            int i = Random.Range(0, 24);
            if (!_roomList[i]._isBomb &&
               (i < _startRoomNum - 1 || i > _startRoomNum + 1) &&
               i != _startRoomNum + _countALine && i != _startRoomNum - _countALine)
            {
                _roomList[i]._isBomb = true;
                array_isbomb[i_isbomb++] = i;

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

            array_aroundBomb[i] = _roomList[i]._aroundBomb;
        }
    }

    [PunRPC]
    void RPC_Sync_isBomb(int[] isbomb, int count)
    {
        for (int i = 0; i < count; i++)
            _roomList[isbomb[i]]._isBomb = true;
    }
    [PunRPC]
    void RPC_Sync_aroundBomb(int[] aroundBomb)
    {
        for (int i = 0; i < aroundBomb.Length; i++)
            _roomList[i]._aroundBomb = aroundBomb[i];
    }
}
