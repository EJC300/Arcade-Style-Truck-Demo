using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUIHandler : MonoBehaviour
{

    public GameController gameController { get { return GameController.instance; } }
    //PauseMenu Pauses Game
    [SerializeField] GameObject pauseMenu;
    private float previousTimeScale = 1f;
    [SerializeField] private Button ExitToMenuButton;

    void ExitToMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("ExitToMenu");
    }

    void PauseGame()
    {

        if (!pauseMenu.activeInHierarchy)
        {
           
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);

        }
        else
        {
            Time.timeScale = previousTimeScale;
            pauseMenu.SetActive(false);

        }
    }

    void OnExitButton()
    {
        if (ExitToMenuButton != null)
        {
            ExitToMenuButton.onClick.AddListener(ExitToMenu);


        }
    }

    private void OnEnable()
    {
        if (gameController != null)
        {
            previousTimeScale = Time.timeScale;
            gameController.exitToMainMenuEvent += OnExitButton;
            gameController.pauseEvent += PauseGame;
            //gameController.unpauseEvent += UnPauseGame;
            pauseMenu.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (gameController != null)
        {
            gameController.exitToMainMenuEvent -= OnExitButton;
            gameController.pauseEvent -= PauseGame;
            // gameController.unpauseEvent -= UnPauseGame;
        }
    }
}
