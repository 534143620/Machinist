using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameLostView : MonoBehaviour
{
    private Button btn_Restart;
    private Button btn_MainMenu;

    private void Awake() {
        btn_MainMenu = transform .Find("Panel/Button_MainMenu") .GetComponent<Button>();
        btn_Restart = transform .Find("Panel/Button_Restart") .GetComponent<Button>();
        btn_MainMenu.onClick.AddListener(OnMainMenuButtonClick);
        btn_Restart.onClick.AddListener(OnRestartButtonClick);
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
