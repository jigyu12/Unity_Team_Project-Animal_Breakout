using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BossTimeLimit : UIElement
{
    [SerializeField] private TMP_Text timerText;
    private float timeRemaining;
    private float SecondsLeftTime = 3f;
    private float TimeOutTextTime = 3f;

    private bool isRunning = false;

    [SerializeField] private GameObject SecondsLefttext;
    [SerializeField] private GameObject TimeOutText;
    private bool isSecondsLeftActive = false;
    private bool isTimeOutTextActive = false;
    private bool hasShownSecondsLeft = false;



    public override void Initialize()
    {
        base.Initialize();
    }

    public void StartTimeOut()
    {

        timeRemaining = 180f;

        isRunning = true;

        hasShownSecondsLeft = false;
        isSecondsLeftActive = false;
        isTimeOutTextActive = false;

        SecondsLefttext.SetActive(false);
        TimeOutText.SetActive(false);
    }

    public void StopTimeOut()
    {
        isRunning = false;
    }
    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                TriggerTimeOut();
            }
            else if (timeRemaining <= 15f && !hasShownSecondsLeft)
            {
                TriggerSecondsLeft();
            }

            UpdateTimerText();
            UpdateSecondsLeft();
        }
        UpdateTimeOutText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }


    private void TriggerSecondsLeft()
    {
        SecondsLeftTime = 3f;
        SecondsLefttext.SetActive(true);
        isSecondsLeftActive = true;
        hasShownSecondsLeft = true; // 한 번만 실행되게
    }
    private void UpdateSecondsLeft()
    {
        if (!isSecondsLeftActive) return;

        SecondsLeftTime -= Time.deltaTime;
        if (SecondsLeftTime <= 0f)
        {
            SecondsLefttext.SetActive(false);
            isSecondsLeftActive = false;
        }
    }

    private void TriggerTimeOut()
    {
        TimeOutTextTime = 3f;
        TimeOutText.SetActive(true);
        isTimeOutTextActive = true;
    }

    private void UpdateTimeOutText()
    {
        if (!isTimeOutTextActive) return;

        TimeOutTextTime -= Time.deltaTime;

        if (TimeOutTextTime <= 0f)
        {
            TimeOutText.SetActive(false);
            isTimeOutTextActive = false;
            isRunning = false;
            gameManager.SetGameState(GameManager_new.GameState.GameOver);
        }
    }

}
