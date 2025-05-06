using UnityEngine;
using TMPro;

public class ScoreUI : UIElement
{
    public TMP_Text scoreText;

    public override void Initialize()
    {
        base.Initialize();
        gameManager.InGameCountManager.ScoreSystem.onScoreChanged += UpdateScoreText;
    }

    private void Start()
    {
        UpdateScoreText(0);
    }


    private void UpdateScoreText(long currentScore)
    {
        scoreText.text = $"Score: {currentScore}";
    }
}
