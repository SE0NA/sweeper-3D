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

    [SerializeField] Light ceilLight;
    [SerializeField] List<Light> _stateLights;
    [SerializeField] GameObject _mapPanel;

    public Transform roomPos;

    GameManager gameManager;
    CanvasManager canvasManager;
    Teleport teleport;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        canvasManager = FindObjectOfType<CanvasManager>();
        teleport = FindObjectOfType<Teleport>();
        _mapPanel.GetComponent<MeshRenderer>().material.color = Color.black;
    }

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
            ceilLight.enabled = true;
            _mapPanel.GetComponent<MeshRenderer>().material.color = Color.red;
            gameManager.GameOver();
        }
        else
        {
            _isOpened = true;
            _mapPanel.GetComponent<MeshRenderer>().material.color = Color.white;
            teleport.ChangeBtnColor(1, _roomNum);
        }
    }
    public void RoomFlag()
    {
        _isFlag = true;
        canvasManager.SetRestBomb(false);
        _mapPanel.GetComponent<MeshRenderer>().material.color = Color.yellow;
        teleport.ChangeBtnColor(2, _roomNum);
        StateLightOn();
    }
    public void RoomUnFlag()
    {
        _isFlag = false;
        canvasManager.SetRestBomb(true);
        _mapPanel.GetComponent<MeshRenderer>().material.color = Color.black;
        teleport.ChangeBtnColor(3, _roomNum);
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
