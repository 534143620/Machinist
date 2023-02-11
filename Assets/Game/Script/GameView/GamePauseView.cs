using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseView : MonoBehaviour
{
    private Button btn_Resume;
    private Button btn_MainMenu;
    private Button btn_Restart;

    private void Awake() {
        btn_Resume = transform .Find("Panel/Button_RESUME") .GetComponent<Button>();
        btn_MainMenu = transform .Find("Panel/Button_MainMenu") .GetComponent<Button>();
        btn_Restart = transform .Find("Panel/Button_Restart") .GetComponent<Button>();
        btn_Resume.onClick.AddListener(OnResumeButtonClick);
        btn_MainMenu.onClick.AddListener(OnMainMenuButtonClick);
        btn_Restart.onClick.AddListener(OnRestartButtonClick);

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
