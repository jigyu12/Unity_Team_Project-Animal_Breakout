using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>
{
    private long maxScore;
    private long currentCoins;

    private long inGameScore;

    private OutGameUIManager outGameUIManager;
    //private GameManager gameManager;
    private GameManager_new gameManager;

    public List<int> runnerIDs = new List<int>();
    // private AnimalDatabase animalDatabase;

    private int startAnimalID = 100301;
    public int StartAnimalID => startAnimalID;
    public static event Action<int> onSetStartAnimalID;
    public static event Action OnSceneChangedToInGame;

    public int MinMapObjectId { get; private set; } = 0;
    public int MinRewardItemId { get; private set; } = 0;
    public int MaxMapObjectId { get; private set; } = 0;
    public int MaxRewardItemId { get; private set; } = 0;

    private void Awake()
    {

        OnMaxMapObjectIdSetHandler(DataTableManager.mapObjectsDataTable.maxId);
        OnMinMapObjectIdSetHandler(DataTableManager.mapObjectsDataTable.minId);
        OnMaxRewardItemIdSetHandler(DataTableManager.rewardItemsDataTable.maxId);
        OnMinRewardItemIdSetHandler(DataTableManager.rewardItemsDataTable.minId);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
        AnimalUnlockPanel.onSetStartAnimalID -= OnSetAnimalID;

        SceneManager.sceneLoaded -= OnChangeSceneHandler;

    }

    private void Start()
    {
        maxScore = 0;
        currentCoins = 0;

        BaseCollisionBehaviour.OnScoreChanged += AddScoreInGame;
        AnimalUnlockPanel.onSetStartAnimalID += OnSetAnimalID;

        SceneManager.sceneLoaded += OnChangeSceneHandler;
    }

    private void OnMaxMapObjectIdSetHandler(int maxMapObjectCount)
    {
        MaxMapObjectId = maxMapObjectCount;
    }

    private void OnMinMapObjectIdSetHandler(int minMapObjectCount)
    {
        MinMapObjectId = minMapObjectCount;
    }

    private void OnMaxRewardItemIdSetHandler(int maxRewardItemCount)
    {
        MaxRewardItemId = maxRewardItemCount;
    }

    private void OnMinRewardItemIdSetHandler(int minRewardItemCount)
    {
        MinRewardItemId = minRewardItemCount;
    }

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "MainTitleSceneCopy")
        {
            TryFindOutGameUIManager();
        }
        else if (SceneManager.GetActiveScene().name == "Run_new")
        {
            TryFindGameManager();
        }

        ClearInGameData();
    }

    private void TryFindOutGameUIManager()
    {
        Debug.Assert(GameObject.FindGameObjectWithTag("OutGameUIManager").TryGetComponent(out outGameUIManager)
        , "Cant find OutGameUIManager");
    }

    private void TryFindGameManager()
    {
        Debug.Assert(GameObject.FindGameObjectWithTag("GameManager").TryGetComponent(out gameManager)
        , "Cant find GameManager");
    }

    private void ClearInGameData()
    {
        inGameScore = 0;
    }

    private void AddScoreInGame(long scoreToAdd)
    {
        inGameScore += scoreToAdd;

        if (inGameScore <= 0)
        {
            inGameScore = 0;

            //gameManager.GameOver();
        }
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        if (GameObject.FindGameObjectWithTag("ScoreUI")?.TryGetComponent(out ScoreUI scoreUI) == true)
        {
            scoreUI.UpdateScore(inGameScore);
        }
    }
    private void OnChangeSceneHandler(Scene scene, LoadSceneMode mode)
    {
        if (maxScore < inGameScore)
        {
            maxScore = inGameScore;
        }

        Debug.Log($"InGameScore : {inGameScore}");
        Debug.Log($"MaxScore : {maxScore}");

        currentCoins += inGameScore / 100;

        Debug.Log($"CurrentCoins To Add : {currentCoins}");

        if (SceneManager.GetActiveScene().name == "MainTitleSceneCopy")
        {
            TryFindOutGameUIManager();
        }
        else if (SceneManager.GetActiveScene().name == "Run_new")
        {
            TryFindGameManager();

            OnSceneChangedToInGame?.Invoke();
        }

        ClearInGameData();

        Time.timeScale = 1;
    }

    private void OnSetAnimalID(int id)
    {
        startAnimalID = id;

        onSetStartAnimalID?.Invoke(id);
    }

    //public void SetRunnerIDs(List<int> ids)
    //{
    //    runnerIDs = new List<int>(ids);
    //}

    //public List<int> GetRunnerIDs()
    //{
    //    return new List<int>(runnerIDs);
    //}

    //public void SetAnimalDatabase(AnimalDatabase db)
    //{
    //    animalDatabase = db;
    //}

    //public AnimalDatabase GetAnimalDatabase()
    //{
    //    return animalDatabase;
    //}
}