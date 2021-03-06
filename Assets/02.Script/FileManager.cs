using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoSingleton<FileManager>
{
    public Save save = null;
    private string SAVE_PATH = "";
    private readonly string SAVE_FILING = "/SaveFile.txt";
    private void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";  /*persistentDataPath (for Android)*/
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
    }
    public void SaveToJson()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";  /*persistentDataPath(for Android)*/
        if (save == null) return;
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILING, json, System.Text.Encoding.UTF8);
    }

    public Save LoadFromJson()
    {
        string json = "";
        if (File.Exists(SAVE_PATH + SAVE_FILING))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILING);
            save = JsonUtility.FromJson<Save>(json);
            return save;
        }
        else
        {
            SaveToJson();
            LoadFromJson();
        }
        return null;
    }

    public void ResetJson()
    {
        save = new Save();
        SaveToJson();
        UIManager.Instance.SetHighestYearText(save.highestYear);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
    private void OnDestroy()
    {
        SaveToJson();
    }
}
