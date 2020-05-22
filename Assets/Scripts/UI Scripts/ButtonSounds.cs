﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public string buttonOnClickSoundName = "ButtonOnClick";
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PlayOnClickSound);
    }

    // Update is called once per frame
    public void PlayOnClickSound()
    {
        AudioManager.Manager.PlaySound(buttonOnClickSoundName);
    }
}
