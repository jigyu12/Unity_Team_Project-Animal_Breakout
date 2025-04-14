using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class PausePanulUI : MonoBehaviour
{
    //게임매니저를 캐싱하지 말고 UI매니저를 통해 게임매니저를 접근하는 방식으로 바꾸세요
    [SerializeField]
    private GameManager_new GameManager;

    public GameObject pausePanel;
    public GameObject RealGiveUpPanel;
    // public TMP_Text countdownText;
    public Button resumeButton;
    public Button GiveUpButton;
    public Button settingsButton;

    public GameUIManager gameUIManager;
    private PlayerManager playerManager;
    private void Start()
    {

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(OnResumeButtonClicked);

        GiveUpButton.onClick.RemoveAllListeners();
        GiveUpButton.onClick.AddListener(OnGiveUpButtonClicked);

        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        gameUIManager = GameManager.UIManager;
        playerManager = GameManager.PlayerManager;

    }


    private void OnResumeButtonClicked()
    {
        gameUIManager.CountDown();
        // StartCoroutine(gameUIManager.ResumeAfterCountdown(gameUIManager.countdownText, playerManager.moveForward));
    }

    // public IEnumerator ResumeWithCountdown(TMP_Text countdownText, GameObject pausePanel)
    // {
    //     uiManager.SetDirectionButtonsInteractable(false);
    //     gameManager.SetTimeScale(0);
    //     pausePanel.SetActive(false);
    //     countdownText.gameObject.SetActive(true);
    //     for (int i = 3; i > 0; i--)
    //     {
    //         countdownText.text = i.ToString();
    //         yield return new WaitForSecondsRealtime(1);
    //     }

    //     countdownText.gameObject.SetActive(false);
    //     gameManager.SetTimeScale(1);
    //     uiManager.SetDirectionButtonsInteractable(true);
    // }

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
