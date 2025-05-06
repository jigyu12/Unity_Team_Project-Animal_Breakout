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
        Debug.Log("!");
        gameUIManager.RestartGame();
    }

    private void OnGoMainClicked()
    {
        Debug.Log("!!!!");
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
        long scoreCount = inGameCountManager.ScoreSystem.GetFinalScore();
        scoreCountText.text = $"{scoreCount}";
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

