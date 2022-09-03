using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class InGameMenuBtn : MonoBehaviourPunCallbacks
{
    [SerializeField] List<AudioClip> clipList;

    public void PlayBtnClip(int num)    // <- PlayerController
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[num]);
    }

    public void GotoMainScene()
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[0]);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main");
        PhotonNetwork.Disconnect();
    }

    public void ContinueGame()
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[0]);
        FindObjectOfType<CanvasManager>().ESCMenu(false);
        FindObjectOfType<PlayerController>().gameContinue();
    }
}
