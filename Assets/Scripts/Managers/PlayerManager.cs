using System.Collections;
using UnityEngine;
using TMPro;
public enum DeathType
{
    None,
    Normal,
    DeathZone
}
public class PlayerManager : InGameManager
{
    public GameObject playerRoot;
    public RelayContinueUI relayContinueUI;
    private PlayerRotator playerRotator;
    public MoveForward moveForward;
    private GameUIManager gameUIManager;
    [ReadOnly]
    public PlayerStatus currentPlayerStatus;
    [ReadOnly]
    public PlayerMove currentPlayerMove;
    [ReadOnly]
    public Animator currentPlayerAnimator;

    private int animalID = 100301;//100301;
    private Vector3 pendingRespawnPosition;
    private Quaternion pendingRespawnRotation;
    private Vector3 pendingForward;
    public DeathType lastDeathType = DeathType.None;
    private bool isDead;
    public bool isInIntroSequence { get; set; } = true;
    public void EndIntroSequence() => isInIntroSequence = false;
    // [SerializeField] public TMP_Text countdownText;
    private void Awake()
    {
        playerRotator = GetComponent<PlayerRotator>();
        gameUIManager = GetComponent<GameUIManager>();

    }
    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => DisablePlayer(currentPlayerStatus));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, () => DisablePlayer(currentPlayerStatus));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => EnablePlayer(currentPlayerStatus));


        // GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, () => ContinuePlayerWithCountdown(gameUIManager.countdownText));


        gameUIManager = GameManager.UIManager;
        gameUIManager.playerManager = this;
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, () => EnablePlayer(currentPlayerStatus));
    }
    private void Start()
    {
        moveForward = playerRoot.GetComponent<MoveForward>();
    }
    public void SetPlayer()
    {
        animalID = GameDataManager.Instance.StartAnimalID;
        Debug.Log($"Set Player Start With Animal ID: {animalID}");

        GameObject prefab = LoadManager.Instance.GetCharacterPrefab(animalID);
        if (prefab != null)
        {
            GameObject character = Instantiate(prefab, playerRoot.transform);
            character.SetActive(true);

            PlayerStatus playerStatus = character.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.Initialize();
                currentPlayerStatus = playerStatus;
                ActivatePlayer(playerStatus);
                Debug.Log($"Player {animalID} spawned successfully.");
            }
            else
            {
                Debug.LogError("PlayerStatus component not found on instantiated character.");
            }

            PlayerMove playerMove = character.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                currentPlayerMove = playerMove;
                playerRotator.SetPlayerMove(currentPlayerMove);
            }
            GameManager.UIManager?.ConnectPlayerMove(currentPlayerMove); // 버튼 연결

            currentPlayerAnimator = character.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogError($"Character prefab not found for ID {animalID}.");
        }
    }
    public void ActivatePlayer(PlayerStatus playerStatus)
    {
        moveForward.enabled = true;
        Debug.Log($"MoveForward enabled for: {playerStatus.name}");
    }
    public void OnPlayerDied(PlayerStatus status)
    {
        Debug.Log($"Player Died: {status.name}");
        // 죽기 전 위치 저장 (DeathZone이 아닌 경우)
        if (lastDeathType != DeathType.DeathZone)
        {
            SetPendingRespawnInfo(moveForward.transform.position, moveForward.transform.rotation, moveForward.transform.forward);
            lastDeathType = DeathType.Normal;
        }

        if (lastDeathType == DeathType.DeathZone)
        {
            currentPlayerMove.canTurn = false;
        }
        StopAllMovements();
        DisablePlayer(status);
        PlayDeathAnimation();
        StartCoroutine(DieAndSwitch(status));
    }


    private void StopAllMovements()
    {
        moveForward.enabled = false;
        Debug.Log("All movements stopped.");
    }

    private void DisablePlayer(PlayerStatus playerStatus)
    {
        currentPlayerMove.DisableInput();  // 입력 비활성화
        // GameManager.UIManager?.SetDirectionButtonsInteractable(false);
    }
    private void EnablePlayer(PlayerStatus playerStatus)
    {

        currentPlayerMove.EnableInput();  // 입력 활성화
        // GameManager.UIManager?.SetDirectionButtonsInteractable(true);
    }

    private void PlayDeathAnimation()
    {
        if (currentPlayerAnimator != null)
        {
            currentPlayerAnimator.SetTrigger("Die");
            Debug.Log("Death animation triggered.");
        }
        else
        {
            Debug.LogError("Animator not found. Unable to play death animation.");
        }
    }
    private void PlayRunAnimaition()
    {
        currentPlayerAnimator.SetTrigger("Run");
    }

    private IEnumerator DieAndSwitch(PlayerStatus playerStatus)
    {
        gameUIManager.SetDirectionButtonsInteractable(false);
        yield return new WaitForSeconds(1.5f);
        GameManager.SetGameState(GameManager_new.GameState.GameStop);
        if (relayContinueUI != null)
        {
            relayContinueUI.Show();
        }
        else
        {
            Debug.LogError("RelayContinueUI를 찾을 수 없습니다!");
            GameManager.OnGameOver();
        }
        //  Destroy(playerStatus.gameObject);
        // Debug.Log($"Player {playerStatus.name} destroyed.");
    }

    public void SetPendingRespawnInfo(Vector3 position, Quaternion rotation, Vector3 forward)
    {
        pendingRespawnPosition = position;
        pendingRespawnRotation = rotation;
        pendingForward = forward.normalized;
    }
    // public void ContinuePlayer()
    // {
    //     if (currentPlayerStatus == null)
    //     {
    //         Debug.LogError("부활할 플레이어가 없습니다.");
    //         return;
    //     }

    //     var moveForward = currentPlayerStatus.GetComponentInParent<MoveForward>();

    //     if (lastDeathType == DeathType.DeathZone)
    //     {
    //         moveForward.transform.SetPositionAndRotation(pendingRespawnPosition, pendingRespawnRotation);
    //         moveForward.SetDirectionByRotation();
    //     }

    //     currentPlayerStatus.SetInvincible(true);

    //     currentPlayerMove.DisableInput();
    //     moveForward.enabled = false;

    //     currentPlayerMove.EnableInput();
    //     currentPlayerAnimator.SetTrigger("Run");
    //     ActivatePlayer(currentPlayerStatus);
    //     Debug.Log("플레이어 부활 및 무적 상태 설정");

    //     StartCoroutine(RemoveInvincibilityAfterDelay(2f)); //2초무적

    //     lastDeathType = DeathType.None;
    // }

    private IEnumerator RemoveInvincibilityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentPlayerStatus != null)
        {
            currentPlayerStatus.SetInvincible(false);
            Debug.Log("무적 상태 해제");
        }
    }
    public void SetLastDeathType(DeathType type)
    {
        lastDeathType = type;
    }
    public void ContinuePlayerWithCountdown(TMP_Text countdownText)
    {
        // GameManager.SetTimeScale(0);

        if (currentPlayerStatus == null)
        {
            Debug.LogError("부활할 플레이어가 없습니다.");
            return;
        }


        if (lastDeathType == DeathType.DeathZone)
        {
            moveForward.transform.SetPositionAndRotation(pendingRespawnPosition, pendingRespawnRotation);
            moveForward.SetDirectionByRotation();
        }

        // 무적 유지 (계속 유지됨)
        //currentPlayerStatus.SetInvincible(true);

        // 이동 및 입력 비활성화
        currentPlayerMove.DisableInput();
        moveForward.enabled = false;

        // 애니메이션은 Run으로
        // currentPlayerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        currentPlayerAnimator.SetTrigger("idle");

        ActivatePlayer(currentPlayerStatus);

        //  gameUIManager.CountDown();

    }

    // private IEnumerator ResumeAfterCountdown(TMP_Text countdoext, MoveForward moveForward)
    // {
    //     GameManager.UIManager?.SetDirectionButtonsInteractable(false);
    //     GameManager.SetTimeScale(0);
    //     countdownText.gameObject.SetActive(true);

    //     for (int i = 3; i > 0; i--)
    //     {
    //         countdownText.text = i.ToString();
    //         yield return new WaitForSecondsRealtime(1);
    //     }

    //     countdownText.gameObject.SetActive(false);
    //     GameManager.SetTimeScale(1);

    //     // 이동 및 입력 복원
    //     GameManager.UIManager?.SetDirectionButtonsInteractable(true);
    //     currentPlayerStatus.SetAlive();
    //     currentPlayerMove.EnableInput();
    //     moveForward.enabled = true;
    //     currentPlayerAnimator.updateMode = AnimatorUpdateMode.Normal; // 스케일 영향 받게 함 임시 처리 라 수정 해야함
    //     currentPlayerAnimator.SetTrigger("Run");

    //     // 무적 해제는 따로 2초 후
    //     StartCoroutine(RemoveInvincibilityAfterDelay(2f));

    //     lastDeathType = DeathType.None;
    //     Debug.Log("플레이어 3초 후 부활 처리 완료");
    // }

}