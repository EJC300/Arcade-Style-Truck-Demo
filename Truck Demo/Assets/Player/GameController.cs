using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
  
/*
    A basic game controller that lets you pause view the instructions and reset level.
*/
    #region UI
        [SerializeField] private GameObject Pause;
    #endregion

    #region GameFlowControls
        //Win
        float currentTime;
        bool pause;
        bool demoOver;
        
    #endregion

    #region Level
    Scene currentScene;

    #endregion

    public void Start()
    {
        currentTime = Time.timeScale;
        currentScene = SceneManager.GetActiveScene();
    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ViewInstructions();
        }

        if(pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = currentTime;
        }
    
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    
    public void ExitGame()
    {
        Application.Quit();
    }

     void ViewInstructions()
    {
       pause = !Pause.activeInHierarchy;
       Pause.SetActive(pause);
    }
}
