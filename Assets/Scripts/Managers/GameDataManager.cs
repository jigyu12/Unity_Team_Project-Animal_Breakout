using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private long inGameScore;
    private long coins;
    private long maxScore;

    private void Awake()
    {
        inGameScore = 0;
        
        // Temporary Code
        coins = Random.Range(100, 10000);
        maxScore = Random.Range(100, 10000);
    }

    private void OnEnable()
    {
        BaseCollisionBehaviour.OnScoreChanged += AddScoreInGame;
    }

    private void OnDisable()
    {
        BaseCollisionBehaviour.OnScoreChanged -= AddScoreInGame;
    }

    private void AddScoreInGame(long scoreToAdd)
    {
        inGameScore += scoreToAdd;

        if (inGameScore <= 0)
        {
            inGameScore = 0;
            
            // Todo : Handle GameOver...
        }
    }
}