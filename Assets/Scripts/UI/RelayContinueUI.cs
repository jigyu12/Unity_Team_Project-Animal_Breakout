using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RelayContinueUI : MonoBehaviour
{
    //private RelayRunManager relayRunManager;
    //게임매니저를 캐싱하지 말고 UI매니저를 통해 게임매니저를 접근하는 방식으로 바꾸세요
    [SerializeField]
    private GameManager_new gameManager;

    public GameObject panel;
    public Slider slider;

    private Coroutine countdown;

    //private void Awake()
    //{
    //    relayRunManager = FindObjectOfType<RelayRunManager>();
    //    gameManager = FindObjectOfType<GameManager>();

    //    if (relayRunManager == null)
    //    {
    //        Debug.LogError("RelayRunManager를 찾을 수 없습니다!");
    //    }

    //    if (gameManager == null)
    //    {
    //        Debug.LogError("GameManager를 찾을 수 없습니다!");
    //    }
    //}

    private void Start()
    {
        gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, Show);
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
        {
            StopCoroutine(countdown);
        }

        gameManager.SetGameState(GameManager_new.GameState.GameReStart);
        //if (relayRunManager != null && relayRunManager.HasNextRunner())
        //{
        //}
        //else
        //{
        //    gameManager.SetGameState(GameManager_new.GameState.GameOver);
        //}
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
            gameManager.SetGameState(GameManager_new.GameState.GameOver);
        }
        else
        {
            Debug.LogError("GameManager가 존재하지 않습니다!");
        }
    }
}
