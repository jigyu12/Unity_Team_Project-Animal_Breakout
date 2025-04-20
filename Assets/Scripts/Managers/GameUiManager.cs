using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameUIManager : InGameManager
{
    public PlayerManager playerManager;
    public RotateButtonController rotateButtonController;
    private bool isPaused = false;
    private GameManager_new.GameState previousStateBeforePause;

    public static event Action onShowGameOverPanel;

    private PauseHandler pauseHandler;
    private ReviveHandler reviveHandler;

    [SerializeField] private PausePanelUI pausePanelUI;
    [SerializeField] private ResultPanelUI resultPanelUI;
    [SerializeField] private PauseButtonUI pauseButtonUI;
    [SerializeField] private RotateButtonUI rotateButtonUI;
    [SerializeField] private InputUIBinder inputUIBinder;

    private void Awake()
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, ShowGameOverPanel);
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => SetDirectionButtonsInteractable(false));
        //GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, () => CountDown());
        // GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => SetDirectionButtonsInteractable(true));

        pauseHandler = new PauseHandler(GameManager, playerManager, pausePanelUI, this);
        reviveHandler = new ReviveHandler(GameManager, playerManager, pausePanelUI, this, pauseHandler, this);
        inputUIBinder.Bind(playerManager.playerMove);

        rotateButtonUI.Initialize(rotateButtonController);
        pausePanelUI.Initialize(this);
        resultPanelUI.Initialize(this);
        pauseButtonUI.Initialize(this);
    }

    public void ConnectPlayerMove(PlayerMove move)
    {
        inputUIBinder.Bind(move);
    }
    public void ShowGameOverPanel()
    {
        onShowGameOverPanel?.Invoke();
        resultPanelUI.Show();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMainTitleButtonClicked()
    {
        SceneManager.LoadScene("MainTitleScene");
    }
    private void OnPauseButtonClicked()
    {
        pauseHandler.TogglePause();
    }
    public void SetPauseButtonInteractable(bool interactable)
    {
        pauseButtonUI.SetInteractable(interactable);
    }

    public void Pause()
    {
        pauseHandler.TogglePause();
    }
    public void SetDirectionButtonsInteractable(bool interactable)
    {
        inputUIBinder.SetInteractable(interactable);
        pauseButtonUI.SetInteractable(interactable);
        rotateButtonUI.SetInteractable(interactable);
    }
    public void ShowRotateButton() => rotateButtonUI.Show();
    public void UnShowRotateButton() => rotateButtonUI.Hide();
    private Coroutine coCountDown = null;

    public void CountDown()
    {
        reviveHandler.StartReviveCountdown();
    }

    public void InGameCountDown()
    {
        reviveHandler.StartInGameReviveCountdown();
    }
    public void SetLastDeathType(DeathType type)
    {
        playerManager.lastDeathType = type;
    }
    public void GoToMainTitle()
    {
        SceneManager.LoadScene("MainTitleScene");
    }

    public void RequestContinue()
    {
        CountDown(); // 부활 시작
    }

    public void RequestGiveUp()
    {
        GameManager.SetGameState(GameManager_new.GameState.GameOver);
    }

}