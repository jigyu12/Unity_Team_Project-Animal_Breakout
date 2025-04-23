using UnityEngine;

public class InGameCountManager : InGameManager
{
    public int coinCount;
    public long ScoreCount;
    public long ExpCount;
    public int ReviveCount;

    public override void Initialize()
    {
        base.Initialize();
    }
    private void Awake()
    {
        BaseCollisionBehaviour.OnCoinAcquired += OnCoinAcquiredHandler;
        BaseCollisionBehaviour.OnScoreChanged += OnScoreAcquiredHandler;
    }

    private void OnDestroy()
    {
        BaseCollisionBehaviour.OnCoinAcquired -= OnCoinAcquiredHandler;
        BaseCollisionBehaviour.OnScoreChanged -= OnScoreAcquiredHandler;
    }

    private void OnCoinAcquiredHandler(int amount)
    {
        coinCount += amount;
        Debug.Log(coinCount);
    }
    private void OnScoreAcquiredHandler(long amount)
    {
        ScoreCount += amount;
        Debug.Log(ScoreCount);

    }

}