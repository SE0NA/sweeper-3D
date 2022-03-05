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
        PhotonNetwork.ConnectUsingSettings();   // ������ ���� ���� �õ�
        btn_start.interactable = false;         // ���۹�ư ��Ȱ��ȭ
        txt_message.text = "������ ���� ���� ��...";
        txt_message.color = Color.blue;
    }

    // ������ ���� ���� ����
    public override void OnConnectedToMaster()
    {
        btn_start.interactable = true;      // ���� ��ư Ȱ��ȭ
        txt_message.text = "�¶��� : ������ ���� �����";
        txt_message.color = Color.yellow;
    }
    // ������ ���� ���� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_start.interactable = false;     // ���� ��ư ��Ȱ��ȭ
        txt_message.text = "�������� : ������ ���� ���� ����\n������ ��...";
        txt_message.color = Color.red;
        PhotonNetwork.ConnectUsingSettings();   // �� ���� �õ�
    }

    // �� ���� �õ�
    public void Connect()
    {
        btn_start.interactable = false;     // �ߺ� ���� ����
        if (PhotonNetwork.IsConnected)      // �̹� ������
        {
            txt_message.text = "�뿡 ����...";
            txt_message.color = Color.blue;
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            txt_message.text = "�������� : ������ ������ ������� ����\n������ ��...";
            txt_message.color = Color.red;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // �� ��X
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        txt_message.text = "���ο� �� ����...";
        txt_message.color = Color.blue;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        txt_message.text = "�� ���� ����!\n���� ����";
        txt_message.color = Color.yellow;
        PhotonNetwork.LoadLevel("Stage");
    }
}
