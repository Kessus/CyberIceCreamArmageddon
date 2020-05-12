using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUi : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
                Resume();
            else
            {
                GetComponent<Canvas>().enabled = true;
                IsGamePaused = true;
                Time.timeScale = 0.0f;
            }
        }
    }

    public void Resume()
    {
        GetComponent<Canvas>().enabled = false;
        IsGamePaused = false;
        Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitToDesktop() => Application.Quit();
}
