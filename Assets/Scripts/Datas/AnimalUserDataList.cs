using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalUserDataList : ISaveLoad
{
    public AnimalUserData CurrentAnimalPlayer
    {
        get;
        private set;
    } = null;

    public int CurrentAnimalID
    {
        get => CurrentAnimalPlayer?.AnimalStatData.AnimalID ?? 0;
    }

    //만약 저장본도 없는 초기 플레이어라면, 이 ID로 시작
    public int initialAnimalID = 100112;

    public IReadOnlyList<AnimalUserData> AnimalUserDatas
    {
        //get => animalUserDatas.Values.ToList();
        get => animalUserDataList;
    }

    private Dictionary<int, AnimalUserData> animalUserDataTable = new();
    private List<AnimalUserData> animalUserDataList = new();

    public AnimalUserDataList()
    {
        Load(SaveLoadSystem.Instance.CurrentData.animalUserDataTableSave);
    }



    public AnimalUserData GetAnimalUserData(int animalID)
    {
        if (!animalUserDataTable.ContainsKey(animalID))
        {
            return null;
        }

        return animalUserDataTable[animalID];
    }

    public void SetCurrentAnimalPlayer(int animalID)
    {
        CurrentAnimalPlayer = GetAnimalUserData(animalID);
    }

    public void UnlockAnimal(int animalID)
    {
        if (!animalUserDataTable.ContainsKey(animalID))
        {
            Debug.Assert(false, $"{animalID} is not contained in animalUserDataTable.");

            return;
        }

        animalUserDataTable[animalID].UnlockAnimal();
    }

    public void Save()
    {
        var saveData = SaveLoadSystem.Instance.CurrentData.animalUserDataTableSave = new();
        saveData.currentAnimalID = CurrentAnimalID;
        foreach (var animal in animalUserDataList)
        {
            animal.Save();
        }
    }


    public void Load()
    {
        //저장을 하면 여기서 불러오는 코드가 있으면 됨
        //임시로 데이터 테이블기준으로 일단 세팅
        string dataPath = "ScriptableData/AnimalStat/Animal_{0}";
        foreach (var animalID in DataTableManager.animalDataTable.Keys)
        {
            var statData = Resources.Load<AnimalStatData>(string.Format(dataPath, animalID));
            var animalUserData = new AnimalUserData(statData);

            if (!animalUserDataTable.ContainsKey(animalID))
            {
                animalUserData.Load();
                animalUserDataTable.Add(animalID, animalUserData);
                animalUserDataList.Add(animalUserData);
            }
            else
            {
                Debug.LogError($"AnimalDataList has {animalID} data already");
            }
        }

        // public int startAnimalID { get; private set; } = 100112;
        //코드를 여기에 이식했다 이것도 데이터가 로드될 수 있을때 변동되어야 하는 코드임
        SetCurrentAnimalPlayer(initialAnimalID);
    }

    public void Load(AnimalUserDataListSave saveData)
    {
        if (saveData == null)
        {
            Load();
            return;
        }

        string dataPath = "ScriptableData/AnimalStat/Animal_{0}";
        foreach (var animalID in DataTableManager.animalDataTable.Keys)
        {
            var statData = Resources.Load<AnimalStatData>(string.Format(dataPath, animalID));
            var animalUserData = new AnimalUserData(statData);

            if (!animalUserDataTable.ContainsKey(animalID))
            {
                if (saveData.animalUserDataTable.ContainsKey(animalID))
                {
                    animalUserData.Load(saveData.animalUserDataTable[animalID]);
                }
                else
                {
                    animalUserData.Load();
                }
                animalUserDataTable.Add(animalID, animalUserData);
                animalUserDataList.Add(animalUserData);
            }
            else
            {
                Debug.LogError($"AnimalDataList has {animalID} data already");
            }
        }

        SetCurrentAnimalPlayer(saveData.currentAnimalID);
    }
}