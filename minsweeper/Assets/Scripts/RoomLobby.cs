using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
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
    }

    public void SetDifficulty()     // only master
    {

    }

    public void Btn_GameStart()     // only master
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;       // Lock this Room

        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            photonView.RPC("RPC_GameStart", RpcTarget.All); // 모든 플레이어 게임 시작
            btn_gameStart.interactable = false;
        }
        else
        {
            txt_noOtherPlayers.GetComponent<Animation>().Play();
        }
    }
    [PunRPC]
    public void RPC_GameStart()
    {
        btn_exit.interactable = false;
        gameObject_Count_gameStart.SetActive(true);
        Invoke("LoadStageScene", 6.0f);
    } 
    void LoadStageScene()
    {
        Debug.Log("Game Start!");
        PhotonNetwork.LoadLevel("Stage");
    }

    public void LeftThisRoom()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("JoinLobby");
    }

}
