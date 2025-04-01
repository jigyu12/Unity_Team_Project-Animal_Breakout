using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RelayContinueUI : MonoBehaviour
{
    private RelayRunManager relayRunManager;
    private GameManager gameManager;

    public GameObject panel;
    public Slider slider;

    private Coroutine countdown;

    private void Awake()
    {
        relayRunManager = FindObjectOfType<RelayRunManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (relayRunManager == null)
        {
            Debug.LogError("RelayRunManager를 찾을 수 없습니다!");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager를 찾을 수 없습니다!");
        }
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

        if (relayRunManager != null && relayRunManager.HasNextRunner())
        {
            relayRunManager.LoadNextRunner();
        }
        else
        {
            gameManager.GameOver();
        }
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
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogError("GameManager가 존재하지 않습니다!");
        }
    }
}
