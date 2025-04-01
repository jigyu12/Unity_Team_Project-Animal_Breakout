using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    private GameManager gameManager;

    public GameObject gameOverPanel;

    public Button mainTitleButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.onGameOver += ShowGameOverPanel;
        }

        gameManager.onGameOver += ShowGameOverPanel;
        
        mainTitleButton.onClick.RemoveAllListeners();
        mainTitleButton.onClick.AddListener(OnMainTitleButtonClicked);

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
        SceneManagerEx.Instance.LoadCurrentScene();
    }

    private void OnMainTitleButtonClicked()
    {
        SceneManagerEx.Instance.LoadScene("MainTitleSceneTest");
    }
}
