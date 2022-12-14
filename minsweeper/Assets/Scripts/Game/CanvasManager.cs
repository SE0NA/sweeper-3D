using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Text txt_timer;
    [SerializeField] Text txt_restBomb;
    [SerializeField] Text txt_scanner;

    [SerializeField] GameObject panel_doorInteraction_enable_flag;
    [SerializeField] GameObject panel_doorInteraction_disable_flag;
    [SerializeField] GameObject panel_nearEnemy;
    Animation panel_nearEnenmy_Animation;
    [SerializeField] GameObject panel_createEnemy;
    [SerializeField] GameObject panel_gameEnd;
    [SerializeField] Text txt_gameEnd;
    [SerializeField] List<Color> scannerTextColor;
    [SerializeField] GameObject panel_escMenu;

    int restBomb;

    public bool _isStopAll = false;
    public int min = 0;
    public float sec = 0;

    GameObject panel_doorInteraction;

    // Start is called before the first frame update
    void Start()
    {
        restBomb = FindObjectOfType<Stage>()._totalBomb;
        SetScannerTo(0);
        txt_restBomb.text = restBomb.ToString();

        panel_nearEnenmy_Animation = panel_nearEnemy.GetComponent<Animation>();

    }

    void Update()
    {
        if (!_isStopAll)
        {
            SetTimer();
        }
    }

    public void SetDoorInteracion_ByFlag(bool enable)
    {
        if (enable) panel_doorInteraction = panel_doorInteraction_enable_flag;
        else        panel_doorInteraction = panel_doorInteraction_disable_flag;
    }

    public void SetScannerTo(int scannerValue)
    {
        txt_scanner.text = scannerValue.ToString();
        txt_scanner.color = new Color(scannerTextColor[scannerValue].r
                                    , scannerTextColor[scannerValue].g
                                    , scannerTextColor[scannerValue].b);
    }

    void SetTimer()
    {
        sec += Time.deltaTime;
        if (sec > 60f)
        {
            sec -= 60f;  min++;
        }
        txt_timer.text = min.ToString() + ":" + ((int)sec).ToString();
    }

    public void SetRestBomb(bool Add)
    {
        if (Add)    restBomb++;
        else        restBomb--;
        txt_restBomb.text = restBomb.ToString();
    }
    public void SetRestBomb(int start)
    {
        restBomb = start;
        txt_restBomb.text = restBomb.ToString();
    }

    public void DoorInteractPanelOn()
    {
        panel_doorInteraction.SetActive(true);
    }
    public void DoorInteractPanelOff()
    {
        panel_doorInteraction.SetActive(false);
    }

    public void NearEnemyPanelOn()
    {
        panel_nearEnenmy_Animation.Play();
    }

    public void CreateEnemyPanelOn()
    {
        panel_createEnemy.SetActive(true);
        panel_createEnemy.GetComponent<Animation>().Play();
        Invoke("CreateEnemyPanelOff", 5f);
    }
    private void CreateEnemyPanelOff()
    {
        panel_createEnemy.SetActive(false);
    }

    public void ESCMenu(bool open)
    {
        if (open)
        {
            panel_escMenu.SetActive(true);
        }
        else
        {
            panel_escMenu.SetActive(false);
        }
    }

    public void GameEndUI(bool isClear)
    {
        _isStopAll = true;
        if (isClear)
        {
            txt_gameEnd.text = "축하드립니다!";
        }
        else
        {
            txt_gameEnd.text = "게임 오버";
        }
        panel_gameEnd.SetActive(true);
    }
}
