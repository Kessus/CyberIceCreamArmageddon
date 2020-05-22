using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneMusic : MonoBehaviour
{
    public string sceneMusicClipName;
    void Start()
    {
        AudioManager.Manager.PlaySound(sceneMusicClipName);
    }
}
