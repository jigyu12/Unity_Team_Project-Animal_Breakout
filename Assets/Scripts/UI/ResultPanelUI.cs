using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPanelUI : UIElement
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button goMainButton;

    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text scoreCountText;
    [SerializeField] private TMP_Text expCountText;

    [SerializeField] private TMP_Text playTimeText;
    [SerializeField] InGameCountManager inGameCountManager;
    [SerializeField] TrackingTime trackingTime;

    public float TrackingTime
    {
        get => trackingTime.PlayTime;
    }

    public override void Initialize()
    {
        //gameUIManager = uiManager;
        base.Initialize();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartClicked);

        goMainButton.onClick.RemoveAllListeners();
        goMainButton.onClick.AddListener(OnGoMainClicked);
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, trackingTime.StartTracking);
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameStop, trackingTime.StopTracking);
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, trackingTime.StopTracking);
        gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, SetReslutValues);
        // panelRoot.SetActive(false);
    }

    public override void Show()
    {
        panelRoot.SetActive(true);
    }
    private void OnRestartClicked()
    {
        gameUIManager.RestartGame();
    }

    private void OnGoMainClicked()
    {
        gameUIManager.OnMainTitleButtonClicked();
    }

    public void SetReslutValues()
    {
        SetCoinCount();
        SetScoreCount();
        SetExpCount();
        SetTimeCount();
    }

    public void SetCoinCount()
    {
        int coinCount = inGameCountManager.coinCount;
        coinCountText.text = $"{coinCount}";
    }
    public void SetScoreCount()
    {
        long baseScore = inGameCountManager.ScoreSystem.Score;
        long additionalScore = inGameCountManager.ScoreSystem.AdditionalScore;
        scoreCountText.text = $"{baseScore} + {additionalScore}";
    }

    public void SetExpCount()
    {
        long expCount = gameManager.PlayerManager.playerExperience.TotalExperienceValue;
        expCountText.text = $"{expCount}";
    }
    public void SetTimeCount()
    {
        playTimeText.text = trackingTime.GetFormattedPlayTime();
    }
}

