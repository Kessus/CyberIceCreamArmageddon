using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject creditsText = null;

    public void StartNewGame() => SceneManager.LoadScene("Tutorial");
    public void LoadGame() => SaveManager.LoadGame();
    public void ShowCredits() => creditsText.GetComponent<Credits>().ShowCredits();
    public void ExitGame() => Application.Quit();
}
