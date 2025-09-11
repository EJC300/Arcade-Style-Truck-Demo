using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUIHandler : MonoBehaviour
{

    public GameController gameController { get {  return GameController.instance; } }
    //PauseMenu Pauses Game
    [SerializeField] GameObject pauseMenu;
    private float previousTimeScale = 1f;
    [SerializeField] private Button ExitToMenuButton;

    void ExitToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    void PauseGame()
    {
  
        if (!pauseMenu.activeInHierarchy)
        {
            Debug.Log("Pause");
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = previousTimeScale;
            pauseMenu.SetActive(false);
            Cursor.visible = false;
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
