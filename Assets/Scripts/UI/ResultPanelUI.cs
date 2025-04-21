using UnityEngine;
using UnityEngine.UI;

public class ResultPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button goMainButton;

    private GameUIManager gameUIManager;

    public void Initialize(GameUIManager uiManager)
    {
        gameUIManager = uiManager;

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartClicked);

        goMainButton.onClick.RemoveAllListeners();
        goMainButton.onClick.AddListener(OnGoMainClicked);
        // panelRoot.SetActive(false);
    }

    public void Show()
    {
        panelRoot.SetActive(true);
    }
    private void OnRestartClicked()
    {
        gameUIManager.RestartGame();
    }

    private void OnGoMainClicked()
    {
        gameUIManager.GoToMainTitle();
    }
}
