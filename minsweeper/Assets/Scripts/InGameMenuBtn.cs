using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuBtn : MonoBehaviour
{
    public void GotoMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    public void ReTryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ContinueGame()
    {
        FindObjectOfType<CanvasManager>().ESCMenu(false);
    }
}
