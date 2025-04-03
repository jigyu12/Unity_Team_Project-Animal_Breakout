using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>
{
    private long maxScore;
    private long currentCoins;

    private long inGameScore;

    private OutGameUIManager outGameUIManager;
    private GameManager gameManager;

    public List<int> runnerIDs = new List<int>();
    private AnimalDatabase animalDatabase;

    private void Awake()
    {
        maxScore = 0;
        currentCoins = 0;
    }

    private void OnEnable()
    {
        BaseCollisionBehaviour.OnScoreChanged += AddScoreInGame;
        SceneManager.sceneLoaded += OnChangeSceneHandler;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
    }

    public void Initialize()
    {
        TryFindOutGameUIManager();

        outGameUIManager.SetMaxScoreText(maxScore);
        outGameUIManager.SetTotalCoinText(currentCoins);

        ClearInGameData();
    }

    private void TryFindOutGameUIManager()
    {
        Debug.Assert(GameObject.FindGameObjectWithTag("OutGameUIManager").TryGetComponent(out outGameUIManager)
        , "Cant find OutGameUIManager");
    }

    private void TryFindOutGameManager()
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

            gameManager.GameOver();
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

        if (SceneManager.GetActiveScene().name == "MainTitleSceneTest")
        {
            TryFindOutGameUIManager();
            
            outGameUIManager.SetMaxScoreText(maxScore);
            outGameUIManager.SetTotalCoinText(currentCoins);
        }
        else if (SceneManager.GetActiveScene().name == "RunCopy")
        {
            TryFindOutGameManager();
        }

        ClearInGameData();

        Time.timeScale = 1;
    }
    public void SetRunnerIDs(List<int> ids)
    {
        runnerIDs = new List<int>(ids);
    }

    public List<int> GetRunnerIDs()
    {
        return new List<int>(runnerIDs);
    }

    public void SetAnimalDatabase(AnimalDatabase db)
    {
        animalDatabase = db;
    }

    public AnimalDatabase GetAnimalDatabase()
    {
        return animalDatabase;
    }
}