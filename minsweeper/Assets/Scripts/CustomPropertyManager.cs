using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CustomPropertyManager : MonoBehaviour
{
    [Header("Set UI: CP")]
    [SerializeField] Toggle cp_enable_flag_toggle;
    [SerializeField] Toggle cp_teleport_state_toggle;
    [SerializeField] Slider cp_totalBomb_slider;
    [SerializeField] Text cp_totalBomb_text;
    [SerializeField] Toggle cp_monster_active_toggle;
    [SerializeField] Toggle cp_monster_sound_toggle;
    [SerializeField] Slider cp_monster_defaultspeed_slider;
    [SerializeField] Text cp_monster_defaultspeed_text;
    [SerializeField] Slider cp_monster_maxspeed_slider;
    [SerializeField] Text cp_monster_maxspeed_text;
    [SerializeField] Slider cp_monster_targetarea_radius_slider;
    [SerializeField] Text cp_monster_targetarea_radius_text;
    [SerializeField] Slider cp_monster_howmanyrooms_Slider;
    [SerializeField] Text cp_monster_howmanyrooms_text;

    [SerializeField] Text text_setList;

    Hashtable CP;
    RoomLobby LobbyManager;

    void Awake()
    {
        LobbyManager = FindObjectOfType<RoomLobby>();
        CP = PhotonNetwork.CurrentRoom.CustomProperties;

        Init_CP();
        SetUI_SetListText_All();
    }

    void Init_CP()
    {
        if (!PhotonNetwork.IsMasterClient) return;

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
        CP.Add("monster_targetarea_radius", 7f);
        CP.Add("monster_howmanyrooms", 10);

        // Stage
        CP.Add("totalBomb", 10);
        CP.Add("startRoomNum", 12);
        for (int i = 0; i < 25; i++)
        {
            CP.Add("isBomb" + i.ToString(), false);
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
    }

    public void SetUI_SetPanel_Master()
    {
        cp_enable_flag_toggle.isOn = (bool)CP["enable_flag"];
        cp_teleport_state_toggle.isOn = (bool)CP["teleport_checkAll"];
        cp_totalBomb_slider.value = (int)CP["totalBomb"];
        cp_totalBomb_text.text = cp_totalBomb_slider.value.ToString();
        cp_monster_active_toggle.isOn = (bool)CP["monster_active"];
        cp_monster_sound_toggle.isOn = (bool)CP["monster_sound"];
        cp_monster_defaultspeed_slider.value = (int)CP["monster_defaultspeed"];
        cp_monster_defaultspeed_text.text = cp_monster_defaultspeed_slider.value.ToString();
        cp_monster_maxspeed_slider.minValue = cp_monster_defaultspeed_slider.value;
        cp_monster_maxspeed_slider.value = (int)CP["monster_maxspeed"];
        cp_monster_maxspeed_text.text = cp_monster_maxspeed_slider.value.ToString();
        cp_monster_targetarea_radius_slider.value = (int)((float)CP["monster_targetarea_radius"]);
        cp_monster_targetarea_radius_text.text = cp_monster_targetarea_radius_slider.value.ToString();
        cp_monster_howmanyrooms_Slider.value = (int)CP["monster_howmanyrooms"];
        cp_monster_howmanyrooms_text.text = cp_monster_howmanyrooms_Slider.value.ToString();
        if (!cp_monster_active_toggle.isOn)
        {
            cp_monster_sound_toggle.interactable = false;
            cp_monster_defaultspeed_slider.interactable = false;
            cp_monster_maxspeed_slider.interactable = false;
            cp_monster_targetarea_radius_slider.interactable = false;
            cp_monster_howmanyrooms_Slider.interactable = false;
        }
    }
    
    public void SetUI_SetListText_All()
    {
        text_setList.text = "<size=13><color=yellow>게임</color></size>\n";
        text_setList.text += " 총 지뢰 수: " + ((int)CP["totalBomb"]).ToString() + "개\n";
        text_setList.text += " 깃발 사용: ";
        if ((bool)CP["enable_flag"])    text_setList.text += "O\n";
        else                            text_setList.text += "X\n";
        text_setList.text += " 모든 방 확인: ";
        if ((bool)CP["teleport_checkAll"])  text_setList.text += "O\n\n";
        else                                text_setList.text += "X\n\n";

        text_setList.text += "<size=13><color=magenta>몬스터</color></size>\n";
        text_setList.text += " 활성화: ";
        if ((bool)CP["monster_active"])     text_setList.text += "O\n";
        else                                text_setList.text += "X\n";
        if ((bool)CP["monster_active"])
        {
            text_setList.text += " 음향 효과: ";
            if ((bool)CP["monster_sound"])  text_setList.text += "O\n";
            else                            text_setList.text += "X\n";
            text_setList.text += " 기본 속도: " + ((int)CP["monster_defaultspeed"]).ToString() + "\n";
            text_setList.text += " 최고 속도: " + ((int)CP["monster_maxspeed"]).ToString() + "\n";
            text_setList.text += " 타겟 탐색 범위: " + ((int)((float)CP["monster_targetarea_radius"])).ToString() + "\n";
            text_setList.text += " 출발 방 개수: " + ((int)CP["monster_howmanyrooms"]).ToString();
        }
    }

    public void Btn_CheckSet_SetCP()
    {
        CP["enable_flag"] = cp_enable_flag_toggle.isOn;
        CP["teleport_checkAll"] = cp_teleport_state_toggle.isOn;
        CP["monster_active"] = cp_monster_active_toggle.isOn;
        CP["monster_sound"] = cp_monster_sound_toggle.isOn;
        CP["monster_defaultspeed"] = (int)cp_monster_defaultspeed_slider.value;
        CP["monster_maxspeed"] = (int)cp_monster_maxspeed_slider.value;
        CP["monster_targetarea_radius"] = cp_monster_targetarea_radius_slider.value;
        CP["monster_howmanyrooms"] = (int)cp_monster_howmanyrooms_Slider.value;
        CP["totalBomb"] = (int)cp_totalBomb_slider.value;

        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
    }

    // 각 UI 활성화 설정
    public void Slider_totalBomb(Slider sd)
    {
        cp_totalBomb_text.text = sd.value.ToString();
        cp_monster_howmanyrooms_Slider.maxValue = 23 - sd.value;
    }

    public void Toggle_monster_active(Toggle tg)
    {
        if (tg.isOn)
        {
            cp_monster_sound_toggle.interactable = true;
            cp_monster_defaultspeed_slider.interactable = true;
            cp_monster_maxspeed_slider.interactable = true;
            cp_monster_targetarea_radius_slider.interactable = true;
            cp_monster_howmanyrooms_Slider.interactable = true;
        }
        else
        {
            cp_monster_sound_toggle.interactable = false;
            cp_monster_defaultspeed_slider.interactable = false;
            cp_monster_maxspeed_slider.interactable = false;
            cp_monster_targetarea_radius_slider.interactable = false;
            cp_monster_howmanyrooms_Slider.interactable = false;
        }
    }
    public void Slider_monster_defalut(Slider sd)
    {
        cp_monster_defaultspeed_text.text = sd.value.ToString();
        cp_monster_maxspeed_slider.minValue = sd.value;
    }
    public void Slider_monster_maxspeed(Slider sd)
    {
        cp_monster_maxspeed_text.text = sd.value.ToString();
    }
    public void Slider_monster_targetarea_radius(Slider sd)
    {
        cp_monster_targetarea_radius_text.text = sd.value.ToString();
    }
    public void Slider_monster_howmanyrooms(Slider sd)
    {
        cp_monster_howmanyrooms_text.text = sd.value.ToString();
    }
}
