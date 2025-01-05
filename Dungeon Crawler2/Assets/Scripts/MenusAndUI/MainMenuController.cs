using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Loads the the main scene
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }
    /// <summary>
    /// Shuts down the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
