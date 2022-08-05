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
    private GameObject _thisEnemy;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSettingPos;
    [SerializeField] GameObject patrolPoints;

    PlayerController player;
    Stage stage;
    CanvasManager canvasManager;
    Teleport teleport;

    public int _openedRoom = 0;

    public bool monster_activation;
    int monster_howmanyrooms;
    bool enable_flag;

    private void Awake()
    {
        stage = FindObjectOfType<Stage>();
        canvasManager = FindObjectOfType<CanvasManager>();
        teleport = FindObjectOfType<Teleport>();

        canvasManager.SetRestBomb((int)PhotonNetwork.CurrentRoom.CustomProperties["totalBomb"]);

        monster_activation = (bool)PhotonNetwork.CurrentRoom.CustomProperties["monster_active"];
        monster_howmanyrooms = (int)PhotonNetwork.CurrentRoom.CustomProperties["monster_howmanyrooms"];
        enable_flag = (bool)PhotonNetwork.CurrentRoom.CustomProperties["enable_flag"];
        teleport.checkAllRoom = (bool)PhotonNetwork.CurrentRoom.CustomProperties["teleport_checkAll"];
        canvasManager.SetDoorInteracion_ByFlag(enable_flag);
    }

    private void Start()
    {
        stage._roomList[stage._startRoomNum].RoomOpen();

        GameObject thisplayer = null;
        thisplayer = PhotonNetwork.Instantiate("Player", playerSettingPos.position, Quaternion.identity);
        thisplayer.transform.position = stage._roomList[12].roomPos.position;
        player = thisplayer.GetComponent<PlayerController>();
        GetComponent<AudioSource>().PlayOneShot(clip_welcome);
    }

    public void CheckOpenedRoomForCreateEnemy()
    {
        if (monster_activation && _openedRoom == monster_howmanyrooms)
        {
            canvasManager.CreateEnemyPanelOn();
            GetComponent<AudioSource>().PlayOneShot(clip_warning_createEnemy);

            if (PhotonNetwork.IsMasterClient)
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
        int createOn = 0;

        // 열리지 않은 방에 몬스터 생성
        while (true)
        {
            createOn = Random.Range(0, stage._roomList.Count - 1);
            if (!stage._roomList[createOn]._isOpened)
                break;
        }
        _thisEnemy = PhotonNetwork.Instantiate("Enemy", playerSettingPos.position, Quaternion.identity);
        _thisEnemy.transform.position = patrolPoints.transform.GetChild(createOn).position;
        _thisEnemy.GetComponent<Enemy>().SetPatrolPointsFromGM(patrolPoints);
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
