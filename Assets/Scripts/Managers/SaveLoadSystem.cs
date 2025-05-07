using Newtonsoft.Json;
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

    public void Save()
    {
        GameDataManager.Instance.AnimalUserDataList.Save();


        var json = JsonConvert.SerializeObject(CurrentData);
        Debug.Log(json);
    }

    public void Load()
    {
        // JsonConvert.DeserializeObject
    }


    private void OnDestroy()
    {

    }
}
