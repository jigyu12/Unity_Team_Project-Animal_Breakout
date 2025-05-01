using UnityEngine;

public class PauseHandler
{
    private GameManager_new gameManager;
    private GameObject pausePanel;
    private PlayerManager playerManager;
    private GameUIManager uiManager;
    private PausePanelUI pausePanelUI;
    private bool isPaused = false;
    private GameManager_new.GameState previousState;

    public PauseHandler(GameManager_new gameManager, PlayerManager playerManager, PausePanelUI pausePanelUI, GameUIManager uiManager)
    {
        this.gameManager = gameManager;
        this.playerManager = playerManager;
        this.pausePanelUI = pausePanelUI;
        this.uiManager = uiManager;
    }
    public void TogglePause()
    {
        if (isPaused) return;

        isPaused = true;
        previousState = gameManager.GetCurrentGameState();
        gameManager.SetGameState(GameManager_new.GameState.GameStop);
        playerManager.playerMove.DisableInput();

        pausePanelUI.Show();
        uiManager.SetDirectionButtonsInteractable(false);
    }
    public void Resume()
    {
        isPaused = false;
        //gameManager.SetGameState(previousState);
        gameManager.RestartGameState();
        playerManager.playerMove.EnableInput();

        pausePanelUI.Hide();
        uiManager.SetDirectionButtonsInteractable(true);

    }


    public bool IsPaused() => isPaused;
}
