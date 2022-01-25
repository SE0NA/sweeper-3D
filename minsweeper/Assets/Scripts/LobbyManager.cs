using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_networkConnectInfo;
    [SerializeField] Button btn_join;
    [SerializeField] InputField if_nickname;
    [SerializeField] InputField if_roomCode;
    [SerializeField] byte maxPlayer;

    [SerializeField] Text txt_playerList;
    [SerializeField] Text txt_playerCount;

    [SerializeField] Text txt_chat;
    [SerializeField] InputField txt_sendChat;

    [SerializeField] Button btn_start;

    [SerializeField] List<Color> color_forNetworkConnectInfo;

    // ����
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        // -> OnConnectedToMaster();
        btn_join.interactable = false;  // ���� ���ӱ��� ��Ȱ��ȭ
    }
    public override void OnConnectedToMaster()
    {
        btn_join.interactable = true;   // ���� ��ư Ȱ��ȭ
        Debug.Log("���� ���� �Ϸ�");
    }
    public override void OnDisconnected(DisconnectCause cause)  // ���� ���� ����
    {
        btn_join.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    // ��
    public void ConnectRoom(bool isRandom)
    {
        btn_join.interactable = false;  // ���� ����
        if (PhotonNetwork.IsConnected)  // ���� ���ӵ� ����
        {
            txt_networkConnectInfo.text = "���� ��...";
            if (isRandom && if_nickname.text.Equals("")) PhotonNetwork.JoinRandomRoom();
            else
            {
                if (if_nickname.text.Equals("") && if_roomCode.text.Equals(""))
                {
                    PhotonNetwork.JoinOrCreateRoom(if_roomCode.text, new RoomOptions { MaxPlayers = maxPlayer }, null);
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        btn_start.enabled = false;
                        btn_start.GetComponent<Image>().color = Color.gray;
                    }
                }
                else
                {
                    txt_networkConnectInfo.text = "��� ������ �Է��� �ּ���!";
                    txt_networkConnectInfo.color = color_forNetworkConnectInfo[2];
                    btn_join.interactable = true;
                }
            }
        }
        else
        {
            txt_networkConnectInfo.text = "�������� : ���� ���� �õ� ��...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinedRoom() => RoomSetting();
    public override void OnMasterClientSwitched(Player newMasterClient) => RoomSetting();
    public override void OnPlayerEnteredRoom(Player newPlayer) => RoomSetting();
    public override void OnPlayerLeftRoom(Player otherPlayer) => RoomSetting();

    private void RoomSetting()  // �� ���� ��ȭ�� �۵��� ����
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            btn_start.enabled = false;
            btn_start.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            btn_start.enabled = true;
            btn_start.GetComponent<Image>().color = Color.white;
        }
        SetPlayerList();
    }

    private void SetPlayerList()
    {
        txt_playerList.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (!PhotonNetwork.PlayerList[i].IsMasterClient)
                txt_playerList.text += PhotonNetwork.PlayerList[i].NickName + "\n";
            else
                txt_playerList.text += "[����] " + PhotonNetwork.PlayerList[i].NickName + "\n";
        }
        txt_playerCount.text = PhotonNetwork.PlayerList.Length + " / " + maxPlayer.ToString();
    }

    [PunRPC]
    private void SetChat(string msg)
    {
        
    }
    public void SendChat()
    {

    }

    public void MasterGameStart()
    {
        PhotonNetwork.LoadLevel("Stage");
    }
}
