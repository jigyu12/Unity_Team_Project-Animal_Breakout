using UnityEngine;
using UnityEngine.UI;

public class ResultPanelUI : UIElement
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button goMainButton;



    public override void Initialize()
    {
        //gameUIManager = uiManager;
        base.Initialize();
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
        Debug.Log("!");
        gameUIManager.RestartGame();
    }

    private void OnGoMainClicked()
    {
        Debug.Log("!!!!");
        gameUIManager.OnMainTitleButtonClicked();
    }
}
