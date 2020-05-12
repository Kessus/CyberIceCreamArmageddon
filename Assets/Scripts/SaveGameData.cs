using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData
{
    public string lastLevelName;

    public SaveGameData(string levelName)
    {
        lastLevelName = levelName;
    }
}
