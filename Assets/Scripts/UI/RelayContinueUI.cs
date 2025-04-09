using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RelayContinueUI : MonoBehaviour
{
    public GameObject panel;
    public Slider slider;
    private Coroutine countdown;
    public TMP_Text countdownText;
    public PlayerManager playerManager;
    [SerializeField] private GameManager_new GameManager;
    private bool isDisplayed = false;
    private int deathCount = 0;
    // public override void Initialize()
    // {
    //     base.Initialize();
    //     GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, Show);
    // }
    public void Start()
    {
        //GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, Show);
    }

    public void Show()
    {
        if (deathCount > 0)
        {
            GameManager.SetGameState(GameManager_new.GameState.GameOver);
            return;
        }
        if (isDisplayed) return;
        deathCount++; ;
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
        StartCoroutine(ResumeWithCountdown(countdownText));
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
    public IEnumerator ResumeWithCountdown(TMP_Text countdownText)
    {
        GameManager.SetTimeScale(0);
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        countdownText.gameObject.SetActive(false);
        GameManager.SetTimeScale(1);

    }
}
