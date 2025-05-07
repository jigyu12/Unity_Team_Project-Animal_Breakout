using System;
using UnityEngine;

public class ScoreSystem
{
    public long Score
    {
        get;
        private set;
    }

    public Action<long> onScoreChanged;
    private float additionalFinalScoreRate = 0f;

    public long AdditionalScore
    {
        get => Mathf.FloorToInt(Score * additionalFinalScoreRate);
    }

    public long GetFinalScore()
    {
        return Score + AdditionalScore;
    }

    public void AddScore(int value)
    {
        Score += (long)value;
        onScoreChanged?.Invoke(Score);
    }

    public void AddAdditionalFinalScoreRate(float rate)
    {
        additionalFinalScoreRate += rate;
    }
}
