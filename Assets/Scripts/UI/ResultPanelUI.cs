using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ResultPanelUI : MonoBehaviour
{
    public GameObject GameResultPanel;
    public Button ReStartButton;
    public Button GoMainButton;

    //private GameManager gameManager;
    //UI매니저를 통해 게임매니저를 접근하는 방식으로 바꾸세요
    [SerializeField]
    private GameManager_new gameManager;

    void Start()
    {
        ReStartButton.onClick.RemoveAllListeners();
        ReStartButton.onClick.AddListener(OnReStartButtonClicked);

        GoMainButton.onClick.RemoveAllListeners();
        GoMainButton.onClick.AddListener(OnGoMainButtonClicked);
    }

    private void OnReStartButtonClicked()
    {
        gameManager.SetGameState(GameManager_new.GameState.GameReStart);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameResultPanel.SetActive(false);

        Time.timeScale = 1;
    }
    private void OnGoMainButtonClicked()
    {
        SceneManager.LoadScene("MainTitleSceneCopy");

        GameResultPanel.SetActive(false);
    }
}
