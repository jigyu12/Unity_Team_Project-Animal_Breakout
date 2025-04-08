using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : InGameManager
{
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Button mainTitleButton;
    public Button pauseButton;

    private void Awake()
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, ShowGameOverPanel);
    }

    private void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
        //gameManager.onGameOver += ShowGameOverPanel;

        mainTitleButton.onClick.RemoveAllListeners();
        mainTitleButton.onClick.AddListener(OnMainTitleButtonClicked);
        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    //private void OnDestroy()
    //{
    //    //if (gameManager != null)
    //    //{
    //    //    gameManager.onGameOver -= ShowGameOverPanel;
    //    //}
    //}

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // OnMainTitleButtonClicked();
    }

    private void OnMainTitleButtonClicked()
    {
        SceneManager.LoadScene("MainTitleSceneCopyMin");
        Time.timeScale = 1;
    }
    private void OnPauseButtonClicked()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
