using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    Stage stage;
    PlayerController player;

    private void Start()
    {
        stage = FindObjectOfType<Stage>();
        player = FindObjectOfType<PlayerController>();

        // button color
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
    }

    public void TeleportBtnClick(int teleportTo)
    {
        stage._roomList[teleportTo].RoomOpen();
        player.transform.position = stage._roomList[teleportTo].roomPos.position;
        player.CursorLock();
        gameObject.SetActive(false);
    }

    public void ChangeBtnColor(int state, int roomNum) // 1: white(open) 2: yellow(flag) 3: gray(none) 4: red(bomb)
    {
        switch (state) {
            case 1:
                transform.GetChild(roomNum).GetComponent<Image>().color = Color.white;
                break;
            case 2:
                transform.GetChild(roomNum).GetComponent<Image>().color = Color.yellow;
                break;
            case 3:
                transform.GetChild(roomNum).GetComponent<Image>().color = Color.gray;
                break;
        }
    }
}
