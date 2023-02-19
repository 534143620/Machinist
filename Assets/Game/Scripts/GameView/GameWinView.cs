using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameWinView : MonoBehaviour
{
    private Button_UI btn_Restart;
    private Button_UI btn_MainMenu;

    private void Awake() {
        btn_MainMenu = transform .Find("Panel/Button_MainMenu") .GetComponent<Button_UI>();
        btn_Restart = transform .Find("Panel/Button_Restart") .GetComponent<Button_UI>();
        btn_MainMenu.ClickFunc += () =>OnMainMenuButtonClick();
        btn_Restart.ClickFunc += () =>OnRestartButtonClick();
        btn_MainMenu.AddButtonSounds();
        btn_Restart.AddButtonSounds();
        }

    private void OnMainMenuButtonClick()
    {
        EventHandler.CallReturnToMainMenu();
    }

    private void OnRestartButtonClick()
    {
        // EventCenter.Broadcast (EventDefine.PlayClickAudio); // 后续按钮音效
        EventHandler.CallGameRestart();
    }
}
