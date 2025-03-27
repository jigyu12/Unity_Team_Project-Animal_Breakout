using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        GameManager.Instance.onGameOver += ShowGameOverPanel;
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameOver -= ShowGameOverPanel;
    }
}
