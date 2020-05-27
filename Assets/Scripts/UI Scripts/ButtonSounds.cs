using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public string buttonOnClickSoundName = "ButtonOnClick";

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PlayOnClickSound);
    }

    public void PlayOnClickSound()
    {
        AudioManager.Manager.PlaySound(buttonOnClickSoundName);
    }
}
