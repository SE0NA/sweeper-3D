using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    Stage stage;
    CanvasManager canvasManager;
    Teleport teleport;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        stage = FindObjectOfType<Stage>();
        canvasManager = FindObjectOfType<CanvasManager>();
        teleport = FindObjectOfType<Teleport>();

        canvasManager.SetRestBomb(stage._totalBomb);
    }

    private void Start()
    {
        teleport.gameObject.SetActive(false);
        stage._roomList[stage._startRoomNum].RoomOpen();
        player.transform.position = stage._roomList[stage._startRoomNum].roomPos.position;
    }

    public void TeleportUI(bool open)
    {
        if (open)
            teleport.gameObject.SetActive(true);
        else
            teleport.gameObject.SetActive(false);
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
        canvasManager.GameEndUI(true);
        player.PlayerGameClear();
    }
    public void GameOver()
    {
        canvasManager.GameEndUI(false);
        player.PlayerDie();
    }
}
