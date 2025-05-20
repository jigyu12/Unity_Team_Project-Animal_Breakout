using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausePanelUI : UIElement
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsOkbutton;
    [SerializeField] private Button koreanButton;
    [SerializeField] private Button englishButton;


    [SerializeField] private GameObject realGiveUpPanel;
    [SerializeField] private GameObject pausePanelRoot;
    [SerializeField] private GameObject OptionPanel;


    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private ResultPanelUI resultPanelUI;


    public override void Initialize()
    {
        //gameUIManager = uiManager;
        base.Initialize();
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() =>
        {
            OnResumeClicked();
            SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
            if (gameManager.StageManager.IsPlayerInBossStage)
            {
                SoundManager.Instance.PlayBgm(BgmClipId.BossBGM);
            }
            else
            {
                SoundManager.Instance.PlayBgm(BgmClipId.IngameBGM);
            }
        });

        giveUpButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.AddListener(() =>
            {
                OnGiveUpClicked();
                SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
            });
        settingsButton.onClick.RemoveAllListeners();
        // settingsButton.onClick.AddListener(OnSettingsClicked);
        settingsButton.onClick.AddListener(() =>
                 {
                     OnSettingsClicked();
                     SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
                 });
        settingsOkbutton.onClick.RemoveAllListeners();
        // settingsOkbutton.onClick.AddListener(OnSettingOkClikced);
        settingsOkbutton.onClick.AddListener(() =>
              {
                  OnSettingOkClikced();
                  SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
              });

    }
    private void Start()
    {
        koreanButton.onClick.AddListener(() => LocalizationUtility.ChangeLocaleNow("Korean (South Korea) (ko-KR)"));
        englishButton.onClick.AddListener(() => LocalizationUtility.ChangeLocaleNow("English (United States) (en-US)"));
    }

    private void OnResumeClicked()
    {
        gameUIManager.InGameCountDown();
    }

    private void OnGiveUpClicked()
    {
        realGiveUpPanel.SetActive(true);
        resultPanelUI.SetScoreCount();
        resultPanelUI.SetCoinCount();
        resultPanelUI.SetExpCount();
        resultPanelUI.SetTimeCount();
        resultPanelUI.SetRewardTexts();
    }

    private void OnSettingsClicked()
    {
        OptionPanel.SetActive(true);
    }
    private void OnSettingOkClikced()
    {
        OptionPanel.SetActive(false);
    }

    public void Hide()
    {
        pausePanelRoot.SetActive(false);
    }
    public override void Show()
    {
        pausePanelRoot.SetActive(true);
    }
    public void ShowCountdown()
    {
        countdownText.gameObject.SetActive(true);

    }

    public void HideCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void UpdateCountdownText(string text)
    {
        countdownText.text = text;
    }

}
