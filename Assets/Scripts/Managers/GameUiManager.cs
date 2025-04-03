using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{

    private GameManager gameManager;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Button mainTitleButton;
    public Button pauseButton;


    private void Awake()
    {
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.onGameOver += ShowGameOverPanel;
        mainTitleButton.onClick.RemoveAllListeners();
        mainTitleButton.onClick.AddListener(OnMainTitleButtonClicked);
        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);

    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.onGameOver -= ShowGameOverPanel;
        }
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnMainTitleButtonClicked();
    }

    private void OnMainTitleButtonClicked()
    {
        SceneManagerEx.Instance.LoadScene("MainTitleSceneTestMinjae");
    }
    private void OnPauseButtonClicked()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
