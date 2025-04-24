using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameUIManager : InGameManager
{
    public RotateButtonController rotateButtonController;

    public static event Action onShowGameOverPanel;
    private PauseHandler pauseHandler;
    private ReviveHandler reviveHandler;


    private List<UIElement> uiElements;
    public static Action<int> OnGamePlayCounting;

    [SerializeField] private PausePanelUI pausePanelUI;
    [SerializeField] private ResultPanelUI resultPanelUI;
    [SerializeField] private PauseButtonUI pauseButtonUI;
    [SerializeField] private RotateButtonUI rotateButtonUI;
    [SerializeField] private InputUIBinder inputUIBinder;
    [SerializeField] public RunStageUI runStageUI;
    [SerializeField] public BossWayUI bossWayUI;


    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, ShowGameOverPanel);
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => SetDirectionButtonsInteractable(false));
        //GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, () => CountDown());
        // GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => SetDirectionButtonsInteractable(true));

        pauseHandler = new PauseHandler(GameManager, GameManager.PlayerManager, pausePanelUI, this);
        reviveHandler = new ReviveHandler(GameManager, GameManager.PlayerManager, pausePanelUI, this, pauseHandler, this);
        inputUIBinder.Bind(GameManager.PlayerManager.playerMove);

        //자식으로 딸린 모든 UIElements들을 탐색한다.
        var elements = gameObject.GetComponentsInChildren<UIElement>(true);
        uiElements = elements.ToList();
        //UIElements들에 매니저를 세팅
        foreach (var element in uiElements)
        {
            element.SetUIManager(GameManager, this);
        }
        //Start보다 이전에 값을 세팅해야하는 UI들의 UIElements은 Initialze에서 해주면 된다
        foreach (var element in uiElements)
        {
            element.Initialize();
        }
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

    public void OnMainTitleButtonClicked()
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
    // private Coroutine coCountDown = null;

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
        GameManager.PlayerManager.lastDeathType = type;
    }


    public void RequestContinue()
    {
        CountDown(); // 부활 시작
    }

    public void RequestGiveUp()
    {
        GameManager.SetGameState(GameManager_new.GameState.GameOver);
        OnGamePlayCounting?.Invoke(1);

    }

}