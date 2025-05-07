using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ReviveContinueUI : UIElement
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button ReviveYesButton;
    [SerializeField] private Button ReviveNoButton;


    [SerializeField] private Slider slider;

    public override void Initialize()
    {
        base.Initialize();

        ReviveYesButton.onClick.RemoveAllListeners();
        ReviveYesButton.onClick.AddListener(() => OnClickContinue());
        ReviveNoButton.onClick.RemoveAllListeners();
        ReviveNoButton.onClick.AddListener(() => OnClickGiveUp());
    }
    private Coroutine countdown;
    private bool isDisplayed = false;
    private int deathCount = 0;

    public override void Show()
    {
        if (deathCount >= gameManager.restartChanceCount)
        {
            gameUIManager.RequestGiveUp(); // GameManager 접근 제거
            return;
        }

        if (isDisplayed) return;

        deathCount++;
        panel.SetActive(true);
        isDisplayed = true;

        if (countdown != null)
            StopCoroutine(countdown);

        countdown = StartCoroutine(Countdown());
    }

    public void OnClickContinue()
    {
        Hide();
        gameUIManager.RequestContinue(); // CountDown 대신 요청 방식
    }

    public void OnClickGiveUp()
    {
        Hide();

        gameUIManager.RequestGiveUp(); // GameState 직접 제어 제거
    }

    private IEnumerator Countdown()
    {
        float duration = 5f;
        float time = duration;

        slider.maxValue = duration;
        slider.value = duration;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            slider.value = time;
            yield return null;
        }

        Hide();
        gameUIManager.RequestGiveUp(); // 자동 포기 처리
    }

    private void Hide()
    {
        panel.SetActive(false);
        isDisplayed = false;
        if (countdown != null)
            StopCoroutine(countdown);
    }
}
