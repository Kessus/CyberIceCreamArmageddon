using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActions : MonoBehaviour
{
    void Start()
    {
        SaveManager.SaveGame(SceneManager.GetActiveScene().name);
    }
}
