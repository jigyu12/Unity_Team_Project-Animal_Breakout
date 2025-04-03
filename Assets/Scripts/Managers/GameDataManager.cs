using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>
{
    private long maxScore;
    private long currentCoins;
    
    private long inGameScore;
    
    private OutGameUIManager outGameUIManager;
    private GameManager gameManager;

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
    }

    private void OnChangeSceneHandler(Scene scene, LoadSceneMode mode)
    {
        if (maxScore < inGameScore)
        {
            maxScore = inGameScore;
        }
        
        Debug.Log( $"InGameScore : {inGameScore}");
        Debug.Log( $"MaxScore : {maxScore}");
            
        currentCoins += inGameScore / 100;
        
        Debug.Log( $"CurrentCoins To Add : {currentCoins}");
        
        if (SceneManager.GetActiveScene().name == "MainTitleSceneTest")
        {
            TryFindOutGameUIManager();
        }
        else if (SceneManager.GetActiveScene().name == "RunCopy")
        {
            TryFindOutGameManager();
        }
        
        ClearInGameData();
            
        Time.timeScale = 1;
    }
}