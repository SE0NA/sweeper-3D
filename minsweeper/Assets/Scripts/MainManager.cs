using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;
    Animator uiAnimator;

    private void Start()
    {
        uiAnimator = uiCanvas.GetComponent<Animator>();
    }

    public void Btn_MultiGame()
    {
        uiAnimator.Play("Main_Fade Out");
        Invoke("Game_Invoke", 1f);
    }
    void Game_Invoke() => SceneManager.LoadScene("JoinLobby");

    public void Extra_join()
    {
        SceneManager.LoadScene("Lobby_extra");
    }

    public void Btn_Quit()
    {
        uiAnimator.Play("Main_Fade Out");
        Invoke("Quit_Invoke", 1f);
    }
    void Quit_Invoke() => Application.Quit();
}