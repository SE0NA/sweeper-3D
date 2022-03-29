using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_networkInformation;
    [SerializeField] InputField if_nickname;
    [SerializeField] InputField if_roomCode;
    [SerializeField] Button btn_joinRandom;
    [SerializeField] Button btn_joinRoom;
    [SerializeField] GameObject ui_pop;
    [SerializeField] Text txt_pop;

    [SerializeField] List<Color> txt_colors;

    void Awake()
    {
        txt_networkInformation.text = "오프라인";
        txt_networkInformation.color = txt_colors[1];

        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
    }

    void Start()
    {
        txt_networkInformation.text = "온라인 접속 중...";
        txt_networkInformation.color = txt_colors[0];
        PhotonNetwork.ConnectUsingSettings();   // 서버 접속
    }

    public override void OnConnectedToMaster()
    {
        txt_networkInformation.text = "온라인";
        txt_networkInformation.color = txt_colors[1];

        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
        txt_networkInformation.text = "오프라인";
        txt_networkInformation.color = txt_colors[1];

        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        txt_networkInformation.text = "연결 끊김!\n<color=yellow>재접속 중...</color>";
        txt_networkInformation.color = txt_colors[3];
        PhotonNetwork.ConnectUsingSettings();
    }

    // 랜덤방
    public void JoinRandomRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>닉네임</color>을 확인해주세요.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "연결 끊김!\n<color=yellow>재접속 중...</color>";
            txt_networkInformation.color = txt_colors[3];
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "랜덤 방 입장 대기 중...";
        txt_networkInformation.color = txt_colors[0];

        PhotonNetwork.LocalPlayer.NickName = nick;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        txt_networkInformation.text = "새로운 방 생성 중...";
        txt_networkInformation.color = txt_colors[1];
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 코드 입력 방 참가
    public void JoinRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>닉네임</color>을 입력해주세요.");
            return;
        }
        if (if_roomCode.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>방코드</color>를 입력해주세요.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "연결 끊김!\n<color=yellow>재접속 중...</color>";
            txt_networkInformation.color = txt_colors[3];
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "방 입장 대기 중...";
        txt_networkInformation.color = txt_colors[0];

        PhotonNetwork.LocalPlayer.NickName = nick;
        PhotonNetwork.JoinOrCreateRoom(if_roomCode.text, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        txt_networkInformation.text = "입장 완료!";
        txt_networkInformation.color = txt_colors[2];
        PhotonNetwork.LoadLevel("RoomLobby");
    }

    public void LoadMain()
    {
        Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void CreatePop(string msg)
    {
        ui_pop.SetActive(true);
        txt_pop.text = msg;
    }

    public void OffPop()
    {
        ui_pop.SetActive(false);
    }
}
