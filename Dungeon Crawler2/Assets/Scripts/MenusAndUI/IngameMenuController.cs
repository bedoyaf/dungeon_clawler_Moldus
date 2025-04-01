using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CurrentMenu
{
    None,
    Pause,
    GameOver,
    Upgrade
}

/// <summary>
/// The class handles mainly menus that pop up in the game, that means the Pause and GameOverMenu
/// </summary>
public class IngameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] public UnityEvent onRestart;
    public CurrentMenu currentMenu = CurrentMenu.None;

    /// <summary>
    /// Just checks if Esc was pressed, then pauses/unpauses
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == CurrentMenu.Pause)
                ResumeGame();
            else if(currentMenu == CurrentMenu.None)
                PauseGame();
        }
    }

    /// <summary>
    /// Freezes the game
    /// </summary>
    public void PauseGame()
    {
        currentMenu = CurrentMenu.Pause;
        Time.timeScale = 0f; 
        pauseMenu.SetActive(true); 
    }
    /// <summary>
    /// Unfrezes the game
    /// </summary>
    public void ResumeGame()
    {
        currentMenu = CurrentMenu.None;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    /// <summary>
    /// Resets and regenerates everithing
    /// </summary>
    public void OnRestartPress()
    {
        Time.timeScale = 1f;
        currentMenu = CurrentMenu.None;
        gameOverScreen.SetActive(false);
        onRestart.Invoke();
    }
    /// <summary>
    /// Shuts down the game
    /// </summary>
    public void OnQuitPress()
    {
        Application.Quit();
    }
    /// <summary>
    /// Just turns on the GameOver state
    /// </summary>
    public void OnDeathGameOverScreen()
    {
        currentMenu = CurrentMenu.GameOver;
    }
}
