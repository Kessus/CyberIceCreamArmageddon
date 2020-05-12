using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    private static BinaryFormatter formatter = new BinaryFormatter();
    private static string filePath = Application.persistentDataPath + "/save.cream";

    //Saves the scene name to a binary file. The name should belong to the most recently opened level.
    public static void SaveGame(string sceneName)
    {
        Debug.Log(filePath);
        SaveGameData save = new SaveGameData(sceneName);
        FileStream stream = new FileStream(filePath, FileMode.Create);
        
        formatter.Serialize(stream, save);
        stream.Close();
    }

    //Opens the last level visited by the player
    public static void LoadGame()
    {
        if (!File.Exists(filePath))
            return;

        FileStream stream = new FileStream(filePath, FileMode.Open);
        if (stream.CanRead)
        {
            SaveGameData loadedSave = formatter.Deserialize(stream) as SaveGameData;
            SceneManager.LoadScene(loadedSave.lastLevelName);
        }
        stream.Close();
    }

    public static bool CanLoadGame()
    {
        return File.Exists(filePath);
    }
}
