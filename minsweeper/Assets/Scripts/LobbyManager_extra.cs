using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager_extra : MonoBehaviourPunCallbacks
{
    public Text txt_message;
    public Button btn_start;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   // 마스터 서버 접속 시도
        btn_start.interactable = false;         // 시작버튼 비활성화
        txt_message.text = "마스터 서버 접속 중...";
        txt_message.color = Color.blue;
    }

    // 마스터 서버 접속 성공
    public override void OnConnectedToMaster()
    {
        btn_start.interactable = true;      // 시작 버튼 활성화
        txt_message.text = "온라인 : 마스터 서버 연결됨";
        txt_message.color = Color.yellow;
    }
    // 마스터 서버 접속 실패
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_start.interactable = false;     // 시작 버튼 비활성화
        txt_message.text = "오프라인 : 마스터 서버 연결 실패\n재접속 중...";
        txt_message.color = Color.red;
        PhotonNetwork.ConnectUsingSettings();   // 재 접속 시도
    }

    // 룸 접속 시도
    public void Connect()
    {
        btn_start.interactable = false;     // 중복 접속 방지
        if (PhotonNetwork.IsConnected)      // 이미 접속중
        {
            txt_message.text = "룸에 접속...";
            txt_message.color = Color.blue;
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            txt_message.text = "오프라인 : 마스터 서버와 연결되지 않음\n재접속 중...";
            txt_message.color = Color.red;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 빈 방X
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        txt_message.text = "새로운 방 생성...";
        txt_message.color = Color.blue;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        txt_message.text = "방 참가 성공!\n게임 시작";
        txt_message.color = Color.yellow;
        PhotonNetwork.LoadLevel("Stage");
    }
}
