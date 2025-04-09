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

    private int startAnimalID = 0;
    public int StartAnimalID => startAnimalID;
    public static event Action<int> onSetStartAnimalID;
    
    private void Start()
    {
        maxScore = 0;
        currentCoins = 0;

        BaseCollisionBehaviour.OnScoreChanged += AddScoreInGame;
        AnimalUnlockPanel.onSetStartAnimalID += OnSetAnimalID;
        
        SceneManager.sceneLoaded += OnChangeSceneHandler;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
        AnimalUnlockPanel.onSetStartAnimalID -= OnSetAnimalID;
        
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
    }

    public void Initialize()
    {
        TryFindOutGameUIManager();

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
        else if (SceneManager.GetActiveScene().name == "RunMin")
        {
            TryFindGameManager();
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