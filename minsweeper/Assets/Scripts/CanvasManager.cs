using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Text txt_timer;
    [SerializeField] Text txt_restBomb;
    [SerializeField] Text txt_scanner;

    [SerializeField] GameObject panel_doorInteraction;
    [SerializeField] List<Color> scannerTextColor;

    int restBomb;

    public bool _isEnd = false;
    public int min = 0;
    public float sec = 0;

    // Start is called before the first frame update
    void Start()
    {
        restBomb = FindObjectOfType<Stage>()._totalBomb;
        SetScannerTo(0);
        txt_restBomb.text = restBomb.ToString();
    }

    void Update()
    {
        if (!_isEnd)
        {
            SetTimer();
        }
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

    public void DoorInteractPanelOn()
    {
        panel_doorInteraction.SetActive(true);
    }
    public void DoorInteractPanelOff()
    {
        panel_doorInteraction.SetActive(false);
    }

    public void GameClearUI()
    {
        _isEnd = true;
    }
    public void GameOverUI()
    {
        _isEnd = true;
    }
}
