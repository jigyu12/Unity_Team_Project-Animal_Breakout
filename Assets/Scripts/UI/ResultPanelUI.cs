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
    [SerializeField] private TMP_Text rewardGoldText;
    [SerializeField] private TMP_Text rewardExpText;
    [SerializeField] private TMP_Text playTimeText;
    private InGameCountManager inGameCountManager;
    private TrackingTime trackingTime;


    public float TrackingTime
    {
        get => trackingTime.PlayTime;
    }

    public override void Initialize()
    {
        inGameCountManager = gameManager.InGameCountManager;
        trackingTime = inGameCountManager.gameObject.GetComponent<TrackingTime>();

        //gameUIManager = uiManager;
        base.Initialize();
        restartButton.onClick.RemoveAllListeners();
        // restartButton.onClick.AddListener(OnRestartClicked);
        restartButton.onClick.AddListener(() =>
                 {
                     OnRestartClicked();
                     SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
                 });
        goMainButton.onClick.RemoveAllListeners();
        goMainButton.onClick.AddListener(OnGoMainClicked);
        goMainButton.onClick.AddListener(() =>
              {
                  OnGoMainClicked();
                  SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
              });
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, trackingTime.StartTracking);
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameStop, trackingTime.StopTracking);
        // gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, trackingTime.StopTracking);
        gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, SetReslutValues);

        GameDataManager.onStaminaChangedInGameDataManager += OnStaminaChanged;
        // panelRoot.SetActive(false);
    }
    private void OnDestroy()
    {
        GameDataManager.onStaminaChangedInGameDataManager -= OnStaminaChanged;
    }
    private void OnStaminaChanged(int currentStamina, int maxStamina)
    {
        UpdateRestartButtonInteractable();
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
        SetRewardTexts();
        UpdateRestartButtonInteractable();
    }
    private void Update()
    {
        UpdateRestartButtonInteractable();
    }

    private void UpdateRestartButtonInteractable()
    {
        int staminaRequiredToRestart = 1; // 필요 행동력 설정
        bool isEnoughStamina = GameDataManager.Instance.StaminaSystem.CurrentStamina >= staminaRequiredToRestart;
        restartButton.interactable = isEnoughStamina;
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

        if (additionalScore > 0)
            scoreCountText.text = $"{baseScore} + {additionalScore}";
        else
            scoreCountText.text = $"{baseScore}";
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
    private void SetRewardTexts()
    {
        long finalScore = inGameCountManager.ScoreSystem.GetFinalScore();
        float playTime = trackingTime.PlayTime;

        long rewardGold = finalScore / 10;
        float bonusRate = GameDataManager.Instance.GetAdditionalScoreGoldRate();
        rewardGold += Mathf.FloorToInt(rewardGold * bonusRate);

        int rewardExp = 40 + Mathf.RoundToInt(playTime * 0.31f);

        rewardGoldText.text = $"{rewardGold}";
        rewardExpText.text = $"{rewardExp}";
    }

}

