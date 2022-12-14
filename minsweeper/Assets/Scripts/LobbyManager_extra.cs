using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        if (PhotonNetwork.IsMasterClient)
        {
            Init_CP();
            SetCP_extra_lobby();
            Debug.Log("setCP");
        }
        PhotonNetwork.LoadLevel("Stage");
    }


    void Init_CP()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        // game
        CP.Add("enable_flag", true);
        CP.Add("teleport_checkAll", true);

        // player
        CP.Add("player_speed", 7f);

        // monster
        CP.Add("monster_active", true);
        CP.Add("monster_sound", true);
        CP.Add("monster_defaultspeed", 2);
        CP.Add("monster_maxspeed", 4);
        CP.Add("monster_sight_distance", 7f);
        CP.Add("monster_sight_angle", 150f);
        CP.Add("monster_targetarea_radius", 15f);
        CP.Add("monster_howmanyrooms", 2);

        // Stage
        CP.Add("totalBomb", 2);
        CP.Add("startRoomNum", 12);
        for (int i = 0; i < 25; i++)
        {
            CP.Add("isBomb" + i.ToString(), false);
        }
    }
    private void SetCP_extra_lobby()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        int count = 0, total, start;
        total = (int)CP["totalBomb"];
        start = (int)CP["startRoomNum"];

        // SetBomb
        while (count < total)
        {
            int i = Random.Range(0, 24);
            if (!(bool)CP["isBomb" + i.ToString()] &&
               (i < start - 1 || i > start + 1) && (i != start + 5 && i != start - 5))
            {
                CP["isBomb" + i.ToString()] = true;
                count++;
            }
        }
    }
}
