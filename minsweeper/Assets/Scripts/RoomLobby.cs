using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_roomCode;
    [SerializeField] Button btn_exit;

    [SerializeField] List<GameObject> gameObject_players;

    [SerializeField] Text txt_chatLog;
    [SerializeField] InputField if_sendChat;

    [SerializeField] Button btn_gameStart;
    [SerializeField] GameObject gameObject_Count_gameStart;
    [SerializeField] GameObject txt_noOtherPlayers;

    [Header("color")]
    [SerializeField] List<Color> color_playerList;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
    //    if (PhotonNetwork.IsMasterClient)
    //        SetCP_Start();
    //    else
        if(!PhotonNetwork.IsMasterClient)
            btn_gameStart.interactable = false;

        if (PhotonNetwork.CurrentRoom.CustomProperties["RoomState"].ToString().Equals("Ramdom"))
            txt_roomCode.text = "Random";
        else
            txt_roomCode.text = PhotonNetwork.CurrentRoom.Name;

        SetPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetPlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetPlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            btn_gameStart.interactable = true;
        }
    }

    public void SetPlayerList()
    {
        int i;
        for(i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player thisplayer = PhotonNetwork.PlayerList[i];
            if (thisplayer.IsMasterClient)                                  // 방장
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.yellow;
            else if(thisplayer.UserId == PhotonNetwork.LocalPlayer.UserId)  // 본인
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.cyan;
            else                                                          // 일반
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.white;
            gameObject_players[i].transform.GetComponentInChildren<Text>().text = thisplayer.NickName;
            gameObject_players[i].GetComponent<Image>().color = color_playerList[0];
        }
        for (; i < 4; i++)  // 빈 슬롯
        {
            gameObject_players[i].GetComponentInChildren<Text>().text = "";
            gameObject_players[i].GetComponent<Image>().color = color_playerList[1];
        }
        Debug.Log("MasterClient: " + PhotonNetwork.MasterClient.NickName);

        // Game Start Button Set
        if (PhotonNetwork.IsMasterClient) {
            if (PhotonNetwork.PlayerList.Length > 1)    // 게임 시작 가능
                btn_gameStart.interactable = true;
            else
                btn_gameStart.interactable = false;
        }

        // 게임 시작 중 인원 변경 시
        if (gameObject_Count_gameStart.activeSelf == true)
        {
            gameObject_Count_gameStart.GetComponent<Animation>().Stop();
            gameObject_Count_gameStart.GetComponent<Animation>().Rewind();
            gameObject_Count_gameStart.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
                CancelInvoke();
        }
    }

    public void SetDifficulty()     // only master
    {

    }

    public void Btn_GameStart()     // only master
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.PlayerList.Length <= 1) {      // 만약의 오류 처리
            btn_gameStart.interactable = false;
            return;
        }

        // 게임 시작
        PhotonNetwork.CurrentRoom.IsOpen = false;       // Lock this Room
        SetCP_GameStart();
        photonView.RPC("PlayAnim_GameStartCount", RpcTarget.AllBuffered);
        Invoke("GameStartByMaster", 5f);

    }
    [PunRPC]
    void PlayAnim_GameStartCount()
    {
        btn_exit.interactable = false;
        gameObject_Count_gameStart.SetActive(true);
        gameObject_Count_gameStart.GetComponent<Animation>().Play();
    }

    void GameStartByMaster()
    {
        PhotonNetwork.LoadLevel("Stage");
    }
    /*
    void SetCP_Start()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        // game
        CP.Add("enable_flag", true);
        CP.Add("monster_active", true);
        CP.Add("monster_sound", true);
        CP.Add("monster_speed", 5f);

        // Stage
        CP.Add("totalBomb", 10);
        CP.Add("startRoomNum", 12);
        for (int i = 0; i < 25; i++)
        {
            CP.Add("isBomb" + i.ToString(), false);
        }
    }
    */
    void SetCP_GameStart()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        int count = 0, total, start;
        total = (int)CP["totalBomb"];
        start = (int)CP["startRoomNum"];

        // SetBomb
        while(count < total)
        {
            int i = Random.Range(0, 24);
            if(!(bool)CP["isBomb"+i.ToString()] &&
               (i < start -1 || i > start +1) && (i != start + 5 && i != start - 5))
            {
                CP["isBomb" + i.ToString()] = true;
                count++;
            }
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
        Debug.Log("RoomLobby - Set Bomb 완료!");
    }

    public void LeftThisRoom()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("JoinLobby");
    }

}
