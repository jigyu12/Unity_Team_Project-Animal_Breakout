using Newtonsoft.Json;
using System.IO;
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

    public SaveDataVC CurrentData
    {
        get;
        set;
    }

    public override void InitializeSingleton()
    {
        base.InitializeSingleton();
        CurrentData = new();
    }

    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        //Converters = { new Vector3Converter(), new QuaternionConverter(), new Vector3IntConverter() }
    };

    private static string SavePathDirectory
    {
        get => $"{Application.persistentDataPath}/Save";
    }

    public void Save()
    {
        GameDataManager.Instance.AnimalUserDataList.Save();

        if (!Directory.Exists(SavePathDirectory))
        {
            Directory.CreateDirectory(SavePathDirectory);
        }
        var path = Path.Combine(SavePathDirectory, "TempJsonFile.json");
        var json = JsonConvert.SerializeObject(CurrentData, settings);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        var path = Path.Combine(SavePathDirectory, "TempJsonFile.json");

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

        CurrentData = saveData as SaveDataVC;
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
