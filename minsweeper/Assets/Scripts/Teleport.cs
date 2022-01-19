using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    Stage stage = null;
    PlayerController player = null;

    private void Start()
    {
        stage = FindObjectOfType<Stage>();
        player = FindObjectOfType<PlayerController>();

        // button color
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
        transform.GetChild(stage._startRoomNum).GetComponent<Image>().color = Color.white;
        
        gameObject.SetActive(false);
    }

    public void TeleportBtnClick(int teleportTo)
    {
        stage._roomList[teleportTo].RoomOpen();
        player.transform.position = stage._roomList[teleportTo].roomPos.position;
        player.CursorLock();
        gameObject.SetActive(false);
    }

    public void TeleportUISetting() // Set button color
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (stage._roomList[i]._isOpened)
                transform.GetChild(i).GetComponent<Image>().color = Color.white;
            else if (stage._roomList[i]._isFlag)
                transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
            else
                transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
        transform.GetChild(player._wherePlayer).GetComponent<Image>().color = Color.green;
    }
}
