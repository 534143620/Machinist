using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character playerCharacter;
    private bool isGameOver;
    void Awake() {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void OnEnable() {
        EventHandler.GameWin += GameWin;
    }
    private void OnDisable() {
        EventHandler.GameWin -= GameWin;
    }

    public void GameOver()
    {
        Debug.Log("GameState = OVER");
    }

    public void GameWin()
    {
        Debug.Log("GameState = WIN");
    }


    void Update()
    {
        if(isGameOver)
            return;
        if(playerCharacter.currentState == Character.CharacterState.Dead){
            isGameOver = true;
            GameOver();
        }
    }
}
