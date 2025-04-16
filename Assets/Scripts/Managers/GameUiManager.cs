using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class GameUIManager : InGameManager
{
    public GameObject gameOverPanel;
    public GameObject GameResultPanel;
    public GameObject pausePanel;
    public Button mainTitleButton;
    public Button RotateButton;

    public Button pauseButton;
    public PlayerManager playerManager;
    public RotateButtonController rotateButtonController;
    private bool isPaused = false;
    private GameManager_new.GameState previousStateBeforePause;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] public TMP_Text countdownText;




    private void Awake()
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, ShowGameOverPanel);
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, () => CountDown());
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => SetDirectionButtonsInteractable(false));
        // GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => SetDirectionButtonsInteractable(true));
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
    public void ConnectPlayerMove(PlayerMove move)
    {
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        leftButton.GetComponent<DirectionButton>().Initialize(move, leftButton);
        rightButton.GetComponent<DirectionButton>().Initialize(move, rightButton);

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
        GameResultPanel.SetActive(true);
        // gameOverPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    public void RestartGame()
    {
        // Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // OnMainTitleButtonClicked();
    }

    private void OnMainTitleButtonClicked()
    {
        SceneManager.LoadScene("MainTitleSceneCopy");
        // Time.timeScale = 1;
    }
    private void OnPauseButtonClicked()
    {
        if (isPaused)
        {
            return;
        }
        isPaused = true;
        previousStateBeforePause = GameManager.GetCurrentGameState();
        GameManager.SetGameState(GameManager_new.GameState.GameStop);
        playerManager.currentPlayerMove.DisableInput();
        pausePanel.SetActive(true);
        SetDirectionButtonsInteractable(false);
        UnShowRotateButton();
    }
    public void SetPauseButtonInteractable(bool interactable)
    {
        pauseButton.interactable = interactable;
    }
    public void SetDirectionButtonsInteractable(bool interactable)
    {
        leftButton.interactable = interactable;
        rightButton.interactable = interactable;
        //   pauseButton.interactable = interactable;
    }
    public void ShowRotateButton()
    {
        // RotateButton.gameObject.SetActive(true);
    }
    public void UnShowRotateButton()
    {
        // RotateButton.gameObject.SetActive(false);
    }





    private Coroutine coCountDown = null;

    public void CountDown()
    {
        if (coCountDown == null)
        {
            coCountDown = StartCoroutine(ResumeAfterCountdown(countdownText, playerManager.moveForward));
        }
        else
        {
            StopCoroutine(coCountDown);
            coCountDown = StartCoroutine(ResumeAfterCountdown(countdownText, playerManager.moveForward));
        }
    }

    public void InGameCountDown()
    {
        if (coCountDown == null)
        {
            coCountDown = StartCoroutine(InGameResumeAfterCountdown(countdownText, playerManager.moveForward));
        }
        else
        {
            StopCoroutine(coCountDown);
            coCountDown = StartCoroutine(InGameResumeAfterCountdown(countdownText, playerManager.moveForward));
        }
    }

    public IEnumerator ResumeAfterCountdown(TMP_Text countdownText, MoveForward moveForward)
    {

        // GameManager.UIManager?.SetDirectionButtonsInteractable(false);
        SetPauseButtonInteractable(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
        countdownText.gameObject.SetActive(true);
        playerManager.currentPlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerManager.currentPlayerAnimator.ResetTrigger("Run");
        playerManager.currentPlayerAnimator.SetTrigger("idle");
        playerManager.currentPlayerStatus.isDead = false; // 임시 죽음 처리 해보기
                                                          // GameManager.SetGameState(GameManager_new.GameState.GameStop);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
            // GameManager.SetGameState(GameManager_new.GameState.GameReStart);
        }
        // currentPlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        // currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal;


        // GameManager.SetTimeScale(1);

        // 이동 및 입력 복원
        // GameManager.UIManager?.SetDirectionButtonsInteractable(true);

        // 무적 해제는 따로 2초 후
        SetPauseButtonInteractable(true);
        isPaused = false;
        if (isPaused == true)
        {
            Debug.Log("Pause 상태라 부활 보류");
            countdownText.gameObject.SetActive(false);
            coCountDown = null;
            yield break;
        }
        // if (previousStateBeforePause == GameManager_new.GameState.GameReady)
        //     GameManager.SetGameState(GameManager_new.GameState.GameReady);
        // else
        SetDirectionButtonsInteractable(true);
        GameManager.SetGameState(GameManager_new.GameState.GamePlay);
        playerManager.currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal;
        countdownText.gameObject.SetActive(false);
        playerManager.currentPlayerStatus.SetAlive();
        playerManager.currentPlayerMove.EnableInput();
        moveForward.enabled = true;
        playerManager.currentPlayerAnimator.SetTrigger("Run");
        StartCoroutine(RemoveInvincibilityAfterDelay(2f));
        coCountDown = null;
        playerManager.lastDeathType = DeathType.None;
        Debug.Log("플레이어 3초 후 부활 처리 완료");
    }

    public IEnumerator InGameResumeAfterCountdown(TMP_Text countdownText, MoveForward moveForward)
    {

        // GameManager.UIManager?.SetDirectionButtonsInteractable(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
        countdownText.gameObject.SetActive(true);
        SetDirectionButtonsInteractable(false);

        // playerManager.currentPlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        // playerManager.currentPlayerAnimator.SetTrigger("idle");
        // GameManager.SetGameState(GameManager_new.GameState.GameStop);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
            // GameManager.SetGameState(GameManager_new.GameState.GameReStart);
        }
        // currentPlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        // currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal;
        // GameManager.SetTimeScale(1);

        // 이동 및 입력 복원
        // GameManager.UIManager?.SetDirectionButtonsInteractable(true);
        // 무적 해제는 따로 2초 후
        isPaused = false;
        // if (previousStateBeforePause == GameManager_new.GameState.GameReady)
        //     GameManager.SetGameState(GameManager_new.GameState.GameReady);
        // else
        if (!playerManager.currentPlayerStatus.isDead && !playerManager.isInIntroSequence)
        {
            SetDirectionButtonsInteractable(true);
            GameManager.SetGameState(GameManager_new.GameState.GamePlay);
            countdownText.gameObject.SetActive(false);
            playerManager.currentPlayerStatus.SetAlive();
            playerManager.currentPlayerMove.EnableInput();
            moveForward.enabled = true;
            playerManager.currentPlayerAnimator.SetTrigger("Run");
            coCountDown = null;
            StartCoroutine(RemoveInvincibilityAfterDelay(2f));
            // playerManager.currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            GameManager.SetGameState(GameManager_new.GameState.GamePlay);
            countdownText.gameObject.SetActive(false);
            coCountDown = null;
        }
        // StartCoroutine(RemoveInvincibilityAfterDelay(2f));

        // playerManager.lastDeathType = DeathType.None;
    }

    private IEnumerator RemoveInvincibilityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerManager.currentPlayerStatus != null)
        {
            playerManager.currentPlayerStatus.SetInvincible(false);
            Debug.Log("무적 상태 해제");
        }
    }
}
