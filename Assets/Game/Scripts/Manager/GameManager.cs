using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Character playerCharacter;
    public GameUI_Manager gameUI_Manager;
    private bool isGameOver;
    void Awake() {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
        gameUI_Manager = GameObject.FindObjectOfType<GameUI_Manager>();
        SoundManager.InitSoundManger();
    }

    private void OnEnable() {
        EventHandler.GameResume += Resume;
        EventHandler.GameRestart += Restart;
        EventHandler.GameToMainMenu += ReturnToMainMenu;
    }
    private void OnDisable() {
        EventHandler.GameResume -= Resume;
        EventHandler.GameRestart -= Restart;
        EventHandler.GameToMainMenu -= ReturnToMainMenu;
    }

    public void GameOver()
    {
        Debug.Log("GameState = LOST");
        gameUI_Manager.ShowGameLostUI();
        SoundManager.PlaySound(SoundManager.Sound.GameLost);
    }

    public void GameWin()
    {
        Debug.Log("GameState = WIN");
        gameUI_Manager.ShowGameWinUI();
        SoundManager.PlaySound(SoundManager.Sound.GameWin);
    }

    void Update()
    {
        if(isGameOver)
            return;
        if(Input.GetKeyDown(KeyCode.Escape))
            gameUI_Manager.TogglePauseUI();
        if(Input.GetKeyDown(KeyCode.Z))
            gameUI_Manager.ShowMinMap();
        if(playerCharacter.currentState == Character.CharacterState.Dead){
            isGameOver = true;
            GameOver();
        }
    }

    public void Resume()
    {
        gameUI_Manager.TogglePauseUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
