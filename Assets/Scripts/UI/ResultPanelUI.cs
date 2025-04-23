using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultPanelUI : UIElement
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button goMainButton;

    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] InGameCountManager inGameCountManager;


    public override void Initialize()
    {
        //gameUIManager = uiManager;
        base.Initialize();
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(OnRestartClicked);

        goMainButton.onClick.RemoveAllListeners();
        goMainButton.onClick.AddListener(OnGoMainClicked);
        gameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, SetCoinCount);

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
    public void SetCoinCount()
    {
        int coinCount = inGameCountManager.coinCount;
        coinCountText.text = $"{coinCount}";
    }
}
