using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuView : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Quit;

    private void Awake() {
        btn_Start = transform .Find("Panel/Button_Start") .GetComponent<Button>();
        btn_Quit = transform .Find("Panel/Button_Quit") .GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStartButtonClick);
        btn_Quit.onClick.AddListener(OnQuitButtonClick);
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
