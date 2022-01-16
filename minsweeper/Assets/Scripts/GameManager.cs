using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Stage stage;

    private void Start()
    {
        stage = FindObjectOfType<Stage>();
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
        FindObjectOfType<CanvasManager>().GameClearUI();
        FindObjectOfType<PlayerController>().PlayerGameClear();
    }
    public void GameOver()
    {
        FindObjectOfType<PlayerController>().PlayerDie();
        FindObjectOfType<CanvasManager>().GameOverUI();
    }
}
