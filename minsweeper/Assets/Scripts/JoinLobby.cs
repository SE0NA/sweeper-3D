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

    [Header("SFX")]
    [SerializeField] AudioSource myAudioSource;
    [SerializeField] AudioClip clip_btn;
    [SerializeField] AudioClip clip_pop;

    Animator uiAnimator;

    void Awake()
    {
        txt_networkInformation.text = "<color=#4C4C4C>" + "��������" + "</color>";

        uiAnimator = canvas.GetComponent<Animator>();

        if (PhotonNetwork.IsConnected)
            Disconnect();
        else {
            btn_joinRandom.interactable = false;
            btn_joinRoom.interactable = false;
        }
    }

    void Start()
    {
        txt_networkInformation.text = "<color=#FFE400>" + "�¶��� ���� ��..." + "</color> ";
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();   // ���� ����
    }

    public override void OnConnectedToMaster()
    {
        txt_networkInformation.text = "<color=#ABF200>" + "�¶���" + "</color> ";  // �¶���

        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }

    public void Disconnect() {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;
        txt_networkInformation.text = "<color=#4C4C4C>" + "��������" + "</color>";

        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        txt_networkInformation.text = "���� ����!\n<color=#FFE400>������ ��...</color>";
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������
    public void JoinRandomRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#FFE400>�г���</color>�� Ȯ�����ּ���.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "<color=#CC3D3D>���� ����!</color>\n<color=#FFE400>������ ��...</color>";
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        myAudioSource.PlayOneShot(clip_btn);
        string nick = if_nickname.text;

        txt_networkInformation.text = "<color=#FFE400>���� �� ���� ��� ��...</color>";

        PhotonNetwork.LocalPlayer.NickName = nick;
        Hashtable cp = new Hashtable { { "RoomState", "Random" } };
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "RoomState", "Random" } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "RoomState" };

        PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "RoomState", "Random" } }
            , expectedMaxPlayers: 4         // ���� ��
            , roomOptions: roomOptions);    // ���� ��
    }

    // �ڵ� �Է� �� ����
    public void JoinRoom()
    {
        btn_joinRandom.interactable = false;
        btn_joinRoom.interactable = false;

        if (if_nickname.text.Length < 1)
        {
            CreatePop("<color=#FFE400>�г���</color>�� �Է����ּ���.");
            return;
        }
        if (if_roomCode.text.Length < 1)
        {
            CreatePop("<color=#B2EBF4>���ڵ�</color>�� �Է����ּ���.");
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            txt_networkInformation.text = "<color=#CC3D3D>���� ����!</color>\n<color=#FFE400>������ ��...</color>";
            PhotonNetwork.ConnectUsingSettings();
            return;
        }

        myAudioSource.PlayOneShot(clip_btn);
        string nick = if_nickname.text;

        txt_networkInformation.text = "<color=#FFE400>�� ���� ��� ��...</color>";

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
        txt_networkInformation.text = "<color=white>���� �Ϸ�!</color>";
        UnityEngine.SceneManagement.SceneManager.LoadScene("RoomLobby");
    }

    public void LoadMain()
    {
        myAudioSource.PlayOneShot(clip_btn);
        uiAnimator.Play("Join_Fade Out");
        Disconnect();
        Invoke("LoadMain_Invoke", 1f);
    }
    void LoadMain_Invoke() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    public void CreatePop(string msg)
    {
        myAudioSource.PlayOneShot(clip_pop);
        Debug.Log("Pop: " + msg);
        ui_pop.SetActive(true);
        txt_pop.text = msg;
    }

    public void OffPop()
    {
        myAudioSource.PlayOneShot(clip_btn);
        ui_pop.SetActive(false);
        btn_joinRandom.interactable = true;
        btn_joinRoom.interactable = true;
    }
}
