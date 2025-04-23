using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public long currentScore;

    private void Start()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    public void UpdateScore(long score)
    {
        currentScore = score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {currentScore}";
    }
}
