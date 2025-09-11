using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
   /*
    * Simple Event driven game state controller
    * Allows for the pause, go back to main menu and quite
    * Will allow for the following:
    * Selection of the three levels and of course pause menu
    * 
    * 
    */
    public static GameController instance;

    //Pause and Unpause
    public delegate void PauseEvent();
    public event PauseEvent pauseEvent;

    public delegate void UnPauseEvent();
    public event PauseEvent unpauseEvent;

    //Quite To Main Menu and Exit Game

    public delegate void ExitToMainMenuEvent();
    public event ExitToMainMenuEvent exitToMainMenuEvent;

    public delegate void ExitGame();
    public event ExitGame exitGameEvent;
    private void OnEnable()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseEvent?.Invoke();
            unpauseEvent?.Invoke();
        }
    }

}
