using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class PausePanulUI : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject RealGiveUpPanel;
    public TMP_Text countdownText;
    public Button resumeButton;
    public Button GiveUpButton;
    public Button settingsButton;
    private GameManager gameManager;

    private void Start()
    {

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(OnResumeButtonClicked);

        GiveUpButton.onClick.RemoveAllListeners();
        GiveUpButton.onClick.AddListener(OnGiveUpButtonClicked);

        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }


    private void OnResumeButtonClicked()
    {
        GameObject gameManagerObject = GameObject.Find("GmManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
            StartCoroutine(gameManager.ResumeWithCountdown(countdownText, pausePanel));
        }
        else
        {
            Debug.LogError("GameManager 오브젝트를 찾을 수 없습니다.");
        }

    }
    private void OnGiveUpButtonClicked()
    {
        RealGiveUpPanel.SetActive(true);
        // SceneManagerEx.Instance.LoadScene("MainTitleSceneTest");
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings 버튼 클릭");
    }

}
