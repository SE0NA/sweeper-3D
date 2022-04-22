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
            if (thisplayer.IsMasterClient)                                  // ����
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.yellow;
            else if(thisplayer.UserId == PhotonNetwork.LocalPlayer.UserId)  // ����
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.cyan;
            else                                                          // �Ϲ�
                gameObject_players[i].transform.GetComponentInChildren<Text>().color = Color.white;
            gameObject_players[i].transform.GetComponentInChildren<Text>().text = thisplayer.NickName;
            gameObject_players[i].GetComponent<Image>().color = color_playerList[0];
        }
        for (; i < 4; i++)  // �� ����
        {
            gameObject_players[i].GetComponentInChildren<Text>().text = "";
            gameObject_players[i].GetComponent<Image>().color = color_playerList[1];
        }
        Debug.Log("MasterClient: " + PhotonNetwork.MasterClient.NickName);

        // Game Start Button Set
        if (PhotonNetwork.IsMasterClient) {
            if (PhotonNetwork.PlayerList.Length > 1)    // ���� ���� ����
                btn_gameStart.interactable = true;
            else
                btn_gameStart.interactable = false;
        }

        // ���� ���� �� �ο� ���� ��
        if (gameObject_Count_gameStart.activeSelf == true)
        {
            gameObject_Count_gameStart.GetComponent<Animation>().Stop();
            gameObject_Count_gameStart.GetComponent<Animation>().Rewind();
            gameObject_Count_gameStart.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
                CancelInvoke();
        }
    }

    public void SetDifficulty()     // only master
    {

    }

    public void Btn_GameStart()     // only master
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.PlayerList.Length <= 1) {      // ������ ���� ó��
            btn_gameStart.interactable = false;
            return;
        }

        // ���� ����
        PhotonNetwork.CurrentRoom.IsOpen = false;       // Lock this Room
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



    public void LeftThisRoom()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("JoinLobby");
    }

}
