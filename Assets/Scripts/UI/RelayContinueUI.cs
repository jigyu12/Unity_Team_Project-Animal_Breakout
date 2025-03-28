using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RelayContinueUI : MonoBehaviour
{
    public static RelayContinueUI Instance { get; private set; }

    public GameObject panel;
    public Slider slider;

    private Coroutine countdown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Show()
    {
        panel.SetActive(true);

        if (countdown != null)
            StopCoroutine(countdown);

        countdown = StartCoroutine(Countdown());
    }

    public void OnClickContinue()
    {
        panel.SetActive(false);

        if (countdown != null)
            StopCoroutine(countdown);

        RelayRunManager.Instance.LoadNextRunner();
    }

    IEnumerator Countdown()
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
        GameManager.Instance.GameOver();
    }
}
