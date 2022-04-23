using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CustomPropertyManager : MonoBehaviour
{
    void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Init_CP();
    }

    void Init_CP()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        // game
        CP.Add("enable_flag", true);
        CP.Add("teleport_checkAll", true);

        // player
        CP.Add("player_speed", 7f);

        // monster
        CP.Add("monster_active", true);
        CP.Add("monster_sound", true);
        CP.Add("monster_defaultspeed", 3);
        CP.Add("monster_maxspeed", 5);
        CP.Add("monster_sight_distance", 7f);
        CP.Add("monster_sight_angle", 150f);
        CP.Add("monster_targetarea_radius", 10f);
        CP.Add("monster_howmanyrooms", 5);

        // Stage
        CP.Add("totalBomb", 10);
        CP.Add("startRoomNum", 12);
        for (int i = 0; i < 25; i++)
        {
            CP.Add("isBomb" + i.ToString(), false);
        }
    }

    public void Set_CP_onLobby()
    {

    }
}
