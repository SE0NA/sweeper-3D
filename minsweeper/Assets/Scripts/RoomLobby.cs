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
    [SerializeField] GameObject txt_single;

    [Header("Game Set")]
    [SerializeField] Button btn_openSet;
    [SerializeField] Button btn_checkSet;
    [SerializeField] GameObject panel_Set;

    [Header("color")]
    [SerializeField] List<Color> color_playerList;

    CustomPropertyManager CPManager;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        CPManager = FindObjectOfType<CustomPropertyManager>();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)   SetUI_Master();
        else                                SetUI_Others();

        if (PhotonNetwork.CurrentRoom.CustomProperties["RoomState"].ToString().Equals("Ramdom"))
            txt_roomCode.text = "Random";
        else
            txt_roomCode.text = PhotonNetwork.CurrentRoom.Name;

        SetPlayerList();
        txt_chatLog.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SendNewChat();
    }

    private void GameStart_Txt_Single()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
            txt_single.SetActive(false);
        else
            txt_single.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetPlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            SetUI_Master();
            GameStart_Txt_Single();
        }
        else SetUI_Others();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetPlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            SetUI_Master();
            GameStart_Txt_Single();
        }
        else SetUI_Others();
    }

    void SetUI_Master()
    {
        btn_gameStart.interactable = true;
        btn_openSet.gameObject.SetActive(true);
        if (PhotonNetwork.CountOfPlayers != 1)
            txt_single.SetActive(false);
        else
            txt_single.SetActive(true);
    }
    void SetUI_Others()
    {
        btn_gameStart.interactable = false;
        btn_openSet.gameObject.SetActive(false);
        txt_single.SetActive(false);
    }
    public void Btn_OpenSet_Master()
    {
        panel_Set.SetActive(true);
        btn_openSet.interactable = false;
        btn_gameStart.interactable = false;
        CPManager.SetUI_SetPanel_Master();
    }

    public void Btn_CheckSet_Master()
    {
        CPManager.Btn_CheckSet_SetCP();
        panel_Set.SetActive(false);
        btn_openSet.interactable = true;
        btn_gameStart.interactable = true;
        photonView.RPC("SetUI_GameSettingText", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void SetUI_GameSettingText()
    {
        CPManager.SetUI_SetListText_All();
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

    void SendNewChat()
    {
        if (if_sendChat.text.Equals("")) return;

        string msg;
        msg = "[<color=cyan>" + PhotonNetwork.LocalPlayer.NickName + "</color>]" + if_sendChat.text;
        ReceiveChat(msg);
        msg = "[" + PhotonNetwork.LocalPlayer.NickName + "]" + if_sendChat.text;
        photonView.RPC("ReceiveChat", RpcTarget.OthersBuffered, msg);
        if_sendChat.text = "";
        if_sendChat.ActivateInputField();
    }
    [PunRPC]
    public void ReceiveChat(string msg)
    {
        if (txt_chatLog.text.Equals(""))    txt_chatLog.text += msg;
        else                                txt_chatLog.text += ("\n" + msg);
    }


    public void Btn_GameStart()     // only master
    {
        if (!PhotonNetwork.IsMasterClient) return;

        btn_gameStart.interactable = false;
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
    }
    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("JoinLobby");
    }

}
