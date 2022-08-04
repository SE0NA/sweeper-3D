using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject uiHelpPanel;
    [SerializeField] Button btn_help;

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

    public void Btn_Help()
    {
        btn_help.interactable = false;
        uiHelpPanel.SetActive(true);
    }
    public void Btn_Help_Ok()
    {
        uiHelpPanel.SetActive(false);
        btn_help.interactable = true;
    }

    public void Btn_Quit()
    {
        uiAnimator.Play("Main_Fade Out");
        Invoke("Quit_Invoke", 1f);
    }
    void Quit_Invoke() => Application.Quit();
}