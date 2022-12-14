using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public void Btn_MultiGame()
    {
        SceneManager.LoadScene("JoinLobby");
    }

    public void Extra_join()
    {
        SceneManager.LoadScene("Lobby_extra");
    }

}