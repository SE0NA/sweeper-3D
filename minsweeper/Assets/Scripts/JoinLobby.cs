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
        txt_networkInformation.text = "��������";
        txt_networkInformation.color = txt_colors[1];

        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
    }

    void Start()
    {
        txt_networkInformation.text = "�¶��� ���� ��...";
        txt_networkInformation.color = txt_colors[0];
        PhotonNetwork.ConnectUsingSettings();   // ���� ����
    }

    public override void OnConnectedToMaster()
    {
        txt_networkInformation.text = "�¶���";
        txt_networkInformation.color = txt_colors[1];

        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
        txt_networkInformation.text = "��������";
        txt_networkInformation.color = txt_colors[1];

        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        txt_networkInformation.text = "���� ����!\n<color=yellow>������ ��...</color>";
        txt_networkInformation.color = txt_colors[3];
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������
    public void JoinRandomRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>�г���</color>�� Ȯ�����ּ���.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "���� ����!\n<color=yellow>������ ��...</color>";
            txt_networkInformation.color = txt_colors[3];
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "���� �� ���� ��� ��...";
        txt_networkInformation.color = txt_colors[0];

        PhotonNetwork.LocalPlayer.NickName = nick;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        txt_networkInformation.text = "���ο� �� ���� ��...";
        txt_networkInformation.color = txt_colors[1];
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // �ڵ� �Է� �� ����
    public void JoinRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>�г���</color>�� �Է����ּ���.");
            return;
        }
        if (if_roomCode.text.Length < 1)
        {
            CreatePop("<color=#D1B2FF>���ڵ�</color>�� �Է����ּ���.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "���� ����!\n<color=yellow>������ ��...</color>";
            txt_networkInformation.color = txt_colors[3];
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        string nick = if_nickname.text;

        txt_networkInformation.text = "�� ���� ��� ��...";
        txt_networkInformation.color = txt_colors[0];

        PhotonNetwork.LocalPlayer.NickName = nick;
        PhotonNetwork.JoinOrCreateRoom(if_roomCode.text, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        txt_networkInformation.text = "���� �Ϸ�!";
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
