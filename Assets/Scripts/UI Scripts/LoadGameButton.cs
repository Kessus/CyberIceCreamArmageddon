using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{
    //Checks if the button should be active or not
    void Start()
    {
        GetComponent<Button>().interactable = SaveManager.CanLoadGame();
    }
}
