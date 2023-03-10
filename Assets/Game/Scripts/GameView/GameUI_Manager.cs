using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI_Manager : MonoBehaviour
{
    public GameManager gameManager;
    public TMPro.TextMeshProUGUI coinText;
    public Slider HealthSlider;
    public enum GameUI_State{
        GamePlay,GamePause,GameWin,GameLost,GameMinMap,GameInventory
    }

    private GameObject UI_GamePause;
    private GameObject UI_GameWin;
    private GameObject UI_GameLost;
    private GameObject UI_GameMinMap;
    private GameObject UI_GameInventory;

    GameUI_State currentState;

    void Start() {
        UI_GamePause = transform.Find("UI_GamePause").gameObject;
        UI_GameWin = transform.Find("UI_GameWin").gameObject;
        UI_GameLost = transform.Find("UI_GameLost").gameObject;
        UI_GameMinMap = transform.Find("UI_GameMinMap").gameObject;
        UI_GameInventory = transform.Find("UI_Inventory").gameObject;
        SwitchUIState(GameUI_State.GamePlay);
    }

    void Update()
    {
        HealthSlider.value = gameManager.playerCharacter.GetComponent<Health>().CurrentHealthPercentage;
        coinText.text = gameManager.playerCharacter.Coin.ToString();
    }
    private void SwitchUIState (GameUI_State state)
    {
        UI_GamePause.SetActive(false);
        UI_GameWin.SetActive(false);
        UI_GameLost.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case GameUI_State.GamePlay:
                break;
            case GameUI_State.GamePause:
                Time.timeScale = 0;
                UI_GamePause.SetActive(true);
                break;
            case GameUI_State.GameWin:
                UI_GameWin.SetActive(true);
                DOTween.Sequence()
                .AppendInterval(2f)
                .AppendCallback(() =>
                {
                   Time.timeScale = 0;
                });
                break;
            case GameUI_State.GameLost:
                UI_GameLost.SetActive(true);
                break;
        }

        currentState = state;
    }

    public void TogglePauseUI()
    {
        SwitchUIState(currentState == GameUI_State.GamePlay ? GameUI_State.GamePause : GameUI_State.GamePlay);
    }

    public void ShowGameLostUI()
    {
        SwitchUIState(GameUI_State.GameLost);
    }

    public void ShowGameWinUI()
    {
        SwitchUIState(GameUI_State.GameWin);
    }

    public void ShowUI(GameUI_State state)
    {
        GameObject ui = null;
        if(state == GameUI_State.GameMinMap)
            ui = UI_GameMinMap;
        else if(state == GameUI_State.GameInventory)
            ui = UI_GameInventory;
        if( ui != null)
            ui.SetActive(!ui.activeSelf);
        else
            Debug.LogError("Invalid GameUI_State!");
    }
}
