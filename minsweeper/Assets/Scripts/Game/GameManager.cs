using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject enemyObject;
    [SerializeField] AudioClip clip_warning_createEnemy;
    [SerializeField] AudioClip clip_welcome;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSettingPos;

    PlayerController player;
    Stage stage;
    CanvasManager canvasManager;
    Teleport teleport;

    public int _openedRoom = 0;
    private int _howManyRoomsForEnemy = 100;

    private void Awake()
    {
        stage = FindObjectOfType<Stage>();
        canvasManager = FindObjectOfType<CanvasManager>();
        teleport = FindObjectOfType<Teleport>();

        canvasManager.SetRestBomb(stage._totalBomb);
    }

    private void Start()
    {
        stage._roomList[stage._startRoomNum].RoomOpen();
        _howManyRoomsForEnemy = stage._howManyRoomsForEnemy;

        GameObject thisplayer = null;
        thisplayer = PhotonNetwork.Instantiate("Player", playerSettingPos.position, Quaternion.identity);
        thisplayer.transform.position = stage._roomList[12].roomPos.position;
        GetComponent<AudioSource>().PlayOneShot(clip_welcome);
    }

    public void CheckOpenedRoomForCreateEnemy()
    {
        if (_openedRoom == _howManyRoomsForEnemy)
        {
            CreateEnemy();
        }
    }

    public void DoorOpenByNum(int rnum, int dnum)
    {
        stage._doorList[dnum].DoorOpen(rnum);
    }
    public void DoorFlagByNum(int rnum, int dnum)
    {
        stage._doorList[dnum].DoorFlag(rnum);
    }

    public void CreateEnemy()
    {
        int createTo = 0;

        for(int i = 0; i < stage._roomList.Count; i++)
        {
            createTo = Random.Range(0, stage._roomList.Count - 1);
            if (createTo != player._wherePlayer)
                break;
        }

        canvasManager.CreateEnemyPanelOn();
        GetComponent<AudioSource>().PlayOneShot(clip_warning_createEnemy);

        enemyObject.SetActive(true);
        enemyObject.transform.position = stage._roomList[createTo].roomPos.position;
    }

    public void TeleportUI(bool open)
    {
        if (open)
        {
            teleport.gameObject.SetActive(true);
            teleport.TeleportUISetting();
        }
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
