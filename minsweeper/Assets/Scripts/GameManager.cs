using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    Stage stage;
    CanvasManager canvasManager;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        stage = FindObjectOfType<Stage>();
        canvasManager = FindObjectOfType<CanvasManager>();

        canvasManager.SetRestBomb(stage._totalBomb);
    }

    private void Start()
    {
        stage._roomList[stage._startRoomNum].RoomOpen();
        TeleportPlayer(stage._startRoomNum);
    }

    public void TeleportPlayer(int roomNum)
    {
        player.transform.position = stage._roomList[roomNum].roomPos.position;
    }

    public void CheckGameClear()    // 지뢰를 제외한 모든 방이 열리면 게임 클리어
    {
        int count = 0;
        for(int i = 0; i < stage._roomList.Count; i++)
        {
            if (stage._roomList[i]._isOpened) count++;
        }
        if (count == stage._roomList.Count - stage._totalBomb)
            GameClear();
    }

    private void GameClear()
    {
        FindObjectOfType<CanvasManager>().GameEndUI(true);
        FindObjectOfType<PlayerController>().PlayerGameClear();
    }
    public void GameOver()
    {
        FindObjectOfType<PlayerController>().PlayerDie();
        FindObjectOfType<CanvasManager>().GameEndUI(false);
    }
}
