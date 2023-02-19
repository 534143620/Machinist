using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GamePauseView : MonoBehaviour
{
    private Button_UI btn_Resume;
    private Button_UI btn_MainMenu;
    private Button_UI btn_Restart;

    private void Awake() {
        btn_Resume = transform .Find("Panel/Button_RESUME") .GetComponent<Button_UI>();
        btn_MainMenu = transform .Find("Panel/Button_MainMenu") .GetComponent<Button_UI>();
        btn_Restart = transform .Find("Panel/Button_Restart") .GetComponent<Button_UI>();
        btn_Resume.ClickFunc += () =>OnResumeButtonClick();
        btn_MainMenu.ClickFunc += () =>OnMainMenuButtonClick();
        btn_Restart.ClickFunc += () =>OnRestartButtonClick();
        btn_Resume.AddButtonSounds();
        btn_MainMenu.AddButtonSounds();
        btn_Restart.AddButtonSounds();
        }

    private void OnResumeButtonClick()
    {
        EventHandler.CallGameResume();
    }

    private void OnRestartButtonClick()
    {
        // EventCenter.Broadcast (EventDefine.PlayClickAudio); // 后续按钮音效
        EventHandler.CallGameRestart();
    }

    private void OnMainMenuButtonClick()
    {
        EventHandler.CallReturnToMainMenu();
    }

}
