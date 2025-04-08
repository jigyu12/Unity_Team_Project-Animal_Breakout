using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RelayContinueUI : InGameManager
{
    public GameObject panel;
    public Slider slider;

    private Coroutine countdown;
    private bool isDisplayed = false;
    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, Show);
    }

    public void Show()
    {
        if (isDisplayed) return;
        panel.SetActive(true);
        isDisplayed = true;

        if (countdown != null)
            StopCoroutine(countdown);

        countdown = StartCoroutine(Countdown());
    }

    public void OnClickContinue()
    {
        panel.SetActive(false);
        isDisplayed = false;
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }

        GameManager.SetGameState(GameManager_new.GameState.GameReStart);
    }

    public void OnClickGiveUp()
    {
        panel.SetActive(false);
        isDisplayed = false;
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }

        GameManager.SetGameState(GameManager_new.GameState.GameOver);
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

        panel.SetActive(false);
        isDisplayed = false;
        GameManager.SetGameState(GameManager_new.GameState.GameOver);
    }
}
