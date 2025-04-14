using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameUIManager : InGameManager
{
    public GameObject gameOverPanel;
    public GameObject GameResultPanel;
    public GameObject pausePanel;
    public Button mainTitleButton;
    public Button pauseButton;
    public PlayerManager playerManager;
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
        pausePanel.SetActive(true);
        playerManager.currentPlayerMove.DisableInput();
        playerManager.moveForward.enabled = false;
        playerManager.currentPlayerAnimator.SetTrigger("idle");
        // SetDirectionButtonsInteractable(false);
        // Time.timeScale = 0;
    }
    public void SetDirectionButtonsInteractable(bool interactable)
    {
        leftButton.interactable = interactable;
        rightButton.interactable = interactable;
        pauseButton.interactable = interactable;
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
    public IEnumerator ResumeAfterCountdown(TMP_Text countdownText, MoveForward moveForward)
    {

        // GameManager.UIManager?.SetDirectionButtonsInteractable(false);
        // GameManager.SetTimeScale(0);

        if (pausePanel != null)
            pausePanel.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        countdownText.gameObject.SetActive(false);
        // GameManager.SetTimeScale(1);

        // 이동 및 입력 복원
        // GameManager.UIManager?.SetDirectionButtonsInteractable(true);
        playerManager.currentPlayerStatus.SetAlive();
        playerManager.currentPlayerMove.EnableInput();
        moveForward.enabled = true;
        // playerManager.currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal; // 스케일 영향 받게 함 임시 처리 라 수정 해야함
        playerManager.currentPlayerAnimator.SetTrigger("Run");


        // 무적 해제는 따로 2초 후
        StartCoroutine(RemoveInvincibilityAfterDelay(2f));
        coCountDown = null;
        playerManager.lastDeathType = DeathType.None;
        Debug.Log("플레이어 3초 후 부활 처리 완료");
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
