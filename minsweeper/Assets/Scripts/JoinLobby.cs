using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_networkInformation;
    [SerializeField] InputField if_nickname;
    [SerializeField] InputField if_roomCode;
    [SerializeField] Button btn_joinRandom;
    [SerializeField] Button btn_joinRoom;
    [SerializeField] GameObject ui_pop;
    [SerializeField] Text txt_pop;

    [SerializeField] GameObject canvas;
    Animator uiAnimator;

    void Awake()
    {
        txt_networkInformation.text = "<color=#4C4C4C>" + "오프라인" + "</color>";

        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        uiAnimator = canvas.GetComponent<Animator>();
    }

    void Start()
    {
        txt_networkInformation.text = "<color=#FFE400>" + "온라인 접속 중..." + "</color> ";
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();   // 서버 접속
    }

    public override void OnConnectedToMaster()
    {
        txt_networkInformation.text = "<color=#ABF200>" + "온라인" + "</color> ";  // 온라인

        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
        txt_networkInformation.text = "<color=#4C4C4C>" + "오프라인" + "</color>";

        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        txt_networkInformation.text = "연결 끊김!\n<color=#FFE400>재접속 중...</color>";
        PhotonNetwork.ConnectUsingSettings();
    }

    // 랜덤방
    public void JoinRandomRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#FFE400>닉네임</color>을 확인해주세요.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "<color=#CC3D3D>연결 끊김!</color>\n<color=#FFE400>재접속 중...</color>";
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "<color=#FFE400>랜덤 방 입장 대기 중...</color>";

        PhotonNetwork.LocalPlayer.NickName = nick;
        Hashtable cp = new Hashtable { { "RoomState", "Random" } };
        PhotonNetwork.JoinRandomRoom(cp, 4);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        txt_networkInformation.text = "<color=#FFE400>새로운 방 생성 중...</color>";

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        roomOptions.CustomRoomProperties = new Hashtable() { { "RoomState", "Ramdom" } };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // 코드 입력 방 참가
    public void JoinRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#FFE400>닉네임</color>을 입력해주세요.");
            return;
        }
        if (if_roomCode.text.Length < 1)
        {
            CreatePop("<color=#B2EBF4>방코드</color>를 입력해주세요.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "<color=#CC3D3D>연결 끊김!</color>\n<color=#FFE400>재접속 중...</color>";
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "<color=#FFE400>방 입장 대기 중...</color>";

        PhotonNetwork.LocalPlayer.NickName = nick;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        roomOptions.CustomRoomProperties = new Hashtable() { { "RoomState", "Static" } };
        PhotonNetwork.JoinOrCreateRoom(if_roomCode.text, roomOptions, null);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " / " + message);
    }

    public override void OnJoinedRoom()
    {
        txt_networkInformation.text = "<color=white>입장 완료!</color>";
        uiAnimator.Play("Join_Fade Out");
        Invoke("JoinRoom_Invoke", 1f);
    }
    void JoinRoom_Invoke() => PhotonNetwork.LoadLevel("RoomLobby");

    public void LoadMain()
    {
        uiAnimator.Play("Join_Fade Out");
        Disconnect();
        Invoke("LoadMain_Invoke", 1f);
    }
    void LoadMain_Invoke() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    public void CreatePop(string msg)
    {
        Debug.Log("Pop: " + msg);
        ui_pop.SetActive(true);
        txt_pop.text = msg;
    }

    public void OffPop()
    {
        ui_pop.SetActive(false);
        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }
}
