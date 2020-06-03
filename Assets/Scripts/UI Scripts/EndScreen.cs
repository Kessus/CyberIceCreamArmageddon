using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject creditsScreen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (creditsScreen.activeSelf)
                creditsScreen.SetActive(false);
            else
                creditsScreen.SetActive(true);
        }
    }
}
