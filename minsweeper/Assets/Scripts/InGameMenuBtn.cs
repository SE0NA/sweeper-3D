using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuBtn : MonoBehaviour
{
    [SerializeField] List<AudioClip> clipList;
    /*
     *   0. button click
     *   1. panel open
     *   2. panel close
     */

    public void PlayBtnClip(int num)
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[num]);
    }

    public void GotoMainScene()
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[0]);
        SceneManager.LoadScene("Main");
    }
    public void ReTryGame()
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[0]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ContinueGame()
    {
        GetComponent<AudioSource>().PlayOneShot(clipList[0]);
        FindObjectOfType<CanvasManager>().ESCMenu(false);
        FindObjectOfType<PlayerController>().gameContinue();
    }
}
