using Newtonsoft.Json;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public class SaveLoadSystem : PersistentMonoSingleton<SaveLoadSystem>
{
    public static int SaveDataVersion
    {
        get;
        private set;
    } = 1;

    public SaveDataVC CurrentSaveData
    {
        get;
        set;
    }
    public static string CurrentSaveFileName
    {
        get => "TempJsonFile.json";
    }

    public static string SavePathDirectory
    {
        get => $"{Application.persistentDataPath}/Save";
    }

    public Action onApplicationQuitSave;

    public override void InitializeSingleton()
    {
        base.InitializeSingleton();
        CurrentSaveData = new();
    }

    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        //Converters = { new Vector3Converter(), new QuaternionConverter(), new Vector3IntConverter() }
    };



    public void Save()
    {
        OnApplicationQuitSave();

        if (!Directory.Exists(SavePathDirectory))
        {
            Directory.CreateDirectory(SavePathDirectory);
        }
        var path = Path.Combine(SavePathDirectory, CurrentSaveFileName);
        var json = JsonConvert.SerializeObject(CurrentSaveData, settings);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        var path = Path.Combine(SavePathDirectory, CurrentSaveFileName);

        if (!File.Exists(path))
        {
            Debug.Log($"save file[{path}] not exist!");
            path = Application.dataPath + "/Resources/Tables/defaultSave.json";
        }

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }

        CurrentSaveData = saveData as SaveDataVC;
    }

    private void OnApplicationQuitSave()
    {
        Debug.Log("Save");
        onApplicationQuitSave?.Invoke();
    }

    public void  RegisterOnSaveAction(ISaveLoad target)
    {
        onApplicationQuitSave += target.Save;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
       if(pause)
       {
           Save();
       }
    }
#endif
}
