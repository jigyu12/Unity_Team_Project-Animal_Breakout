using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausePanelUI : UIElement
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject realGiveUpPanel;
    [SerializeField] private GameObject pausePanelRoot;
    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private ResultPanelUI resultPanelUI;

    public override void Initialize()
    {
        //gameUIManager = uiManager;
        base.Initialize();
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(OnResumeClicked);

        giveUpButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.AddListener(OnGiveUpClicked);

        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsClicked);

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
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Settings 버튼 클릭");
    }

    public void Hide()
    {
        pausePanelRoot.SetActive(false);
    }
    public void Show()
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
