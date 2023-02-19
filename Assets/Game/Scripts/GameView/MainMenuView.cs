using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using CodeMonkey.Utils;

public class MainMenuView : MonoBehaviour
{
    private Button_UI btn_Start;
    private Button_UI btn_Quit;

    private void Awake() {
        btn_Start = transform .Find("Panel/Button_Start") .GetComponent<Button_UI>();
        btn_Quit = transform .Find("Panel/Button_Quit") .GetComponent<Button_UI>();
        btn_Start.ClickFunc += () =>OnStartButtonClick();
        btn_Quit.ClickFunc += () =>OnQuitButtonClick();
        btn_Start.AddButtonSounds(SoundManager.Sound.ButtonEffect2);
        btn_Quit.AddButtonSounds(SoundManager.Sound.ButtonEffect2);
        }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnQuitButtonClick()
    {
        // EventCenter.Broadcast (EventDefine.PlayClickAudio); // 后续按钮音效
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
