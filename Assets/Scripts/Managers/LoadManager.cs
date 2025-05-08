using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LoadManager : Singleton<LoadManager>
{
    private PlayerLoadManager playerLoadManager;


    private void Awake()
    {
        //InitializePlayerLoadManager();
    }

    private void InitializePlayerLoadManager()
    {
        //if (playerLoadManager == null)
        //{
        //    playerLoadManager = FindObjectOfType<PlayerLoadManager>();
        //    if (playerLoadManager == null)
        //    {
        //        GameObject obj = new GameObject("PlayerLoadManager");
        //        playerLoadManager = obj.AddComponent<PlayerLoadManager>();
        //        DontDestroyOnLoad(obj);
        //    }
        //}

        if (playerLoadManager == null)
        {
            playerLoadManager = new PlayerLoadManager();
        }
    }

    // 게임 데이터 전체 로드
    public void LoadGameData(UnityAction onAllLoaded)
    {
        //InitializePlayerLoadManager();
        //Debug.Log("Loading game data...");
        //List<int> runnerIDs = new List<int>();

        //// 데이터 로드
        //AnimalDatabaseLoader loader = FindObjectOfType<AnimalDatabaseLoader>();
        //if (loader != null)
        //{
        //    loader.LoadDataFromCSV();
        //}

        //AnimalDatabase database = GameDataManager.Instance.GetAnimalDatabase();
        //if (database == null)
        //{
        //    Debug.LogError("AnimalDatabase를 찾을 수 없습니다!");
        //    onAllLoaded?.Invoke();
        //    return;
        //}

        //foreach (AnimalStatus animal in database.Animals)
        //{
        //    runnerIDs.Add(animal.AnimalID);
        //}
        //GameDataManager.Instance.SetRunnerIDs(runnerIDs);

        //// 캐릭터 프리로드 요청
        //PreloadCharacters(runnerIDs, onAllLoaded);
    }

    // 캐릭터 프리로드 요청
    public void PreloadCharacters(List<string> prefabNames, UnityAction onAllLoaded = null)
    {
        if (playerLoadManager != null)
        {
            playerLoadManager.PreloadCharacterModels(prefabNames, onAllLoaded);
        }
    }

    // 캐릭터 프리팹 가져오기
    public GameObject GetCharacterPrefab(string prefabName)
    {
        return playerLoadManager.GetLoadedCharacterPrefab(prefabName);
    }

    // 게임 초기화 로직 통합
    public void InitializeGame(UnityAction onInitialized)
    {
        Debug.Log("Initializing Game...");
        //LoadGameData(onInitialized);
    }

    public void LoadInGameResource(UnityAction onInitialized)
    {
        Debug.Log("Load InGame Resource...");

        InitializePlayerLoadManager();
        Debug.Log("Loading game data...");
        // 캐릭터 프리로드 요청

        List<string> prefabNames = new List<string>();
        var animalDataTable = DataTableManager.animalDataTable;
        foreach (int animalID in animalDataTable.GetAnimalIDs())
        {
            prefabNames.Add(animalDataTable.Get(animalID).Prefab);
        }
        PreloadCharacters(prefabNames, onInitialized);
    }
}
