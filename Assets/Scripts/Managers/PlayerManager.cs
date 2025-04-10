using System.Collections;
using UnityEngine;
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

    [ReadOnly]
    public PlayerStatus currentPlayerStatus;
    [ReadOnly]
    public PlayerMove currentPlayerMove;
    [ReadOnly]
    private Animator currentPlayerAnimator;

    private int animalID = 100301;//100301;
    private Vector3 pendingRespawnPosition;
    private Quaternion pendingRespawnRotation;
    private Vector3 pendingForward;
    private DeathType lastDeathType = DeathType.None;

    private void Awake()
    {
        playerRotator = GetComponent<PlayerRotator>();

    }
    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReady, () => DisablePlayer(currentPlayerStatus));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, () => DisablePlayer(currentPlayerStatus));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GamePlay, () => EnablePlayer(currentPlayerStatus));
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameReStart, ContinuePlayer);
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
            currentPlayerAnimator = character.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogError($"Character prefab not found for ID {animalID}.");
        }
    }
    public void ActivatePlayer(PlayerStatus playerStatus)
    {
        MoveForward moveComponent = playerStatus.GetComponentInParent<MoveForward>();
        if (moveComponent != null)
        {
            moveComponent.enabled = true;
            Debug.Log($"MoveForward enabled for: {playerStatus.name}");
        }
    }
    public void OnPlayerDied(PlayerStatus status)
    {
        Debug.Log($"Player Died: {status.name}");

        // 죽기 전 위치 저장 (DeathZone이 아닌 경우)
        if (lastDeathType != DeathType.DeathZone)
        {
            var moveForward = status.GetComponentInParent<MoveForward>();
            SetPendingRespawnInfo(moveForward.transform.position, moveForward.transform.rotation, moveForward.transform.forward);
            lastDeathType = DeathType.Normal;
        }

        StopAllMovements();
        DisablePlayer(status);
        PlayDeathAnimation();
        StartCoroutine(DieAndSwitch(status));
    }


    private void StopAllMovements()
    {
        MoveForward[] movingObjects = FindObjectsOfType<MoveForward>();
        foreach (var move in movingObjects)
        {
            move.enabled = false;
        }
        Debug.Log("All movements stopped.");
    }

    private void DisablePlayer(PlayerStatus playerStatus)
    {

        currentPlayerMove.DisableInput();  // 입력 비활성화

    }
    private void EnablePlayer(PlayerStatus playerStatus)
    {

        currentPlayerMove.EnableInput();  // 입력 비활성화

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

    private IEnumerator DieAndSwitch(PlayerStatus playerStatus)
    {
        yield return new WaitForSeconds(1.5f);
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
    public void ContinuePlayer()
    {
        if (currentPlayerStatus == null)
        {
            Debug.LogError("부활할 플레이어가 없습니다.");
            return;
        }

        var moveForward = currentPlayerStatus.GetComponentInParent<MoveForward>();

        if (lastDeathType == DeathType.DeathZone)
        {
            moveForward.transform.SetPositionAndRotation(pendingRespawnPosition, pendingRespawnRotation);
            moveForward.SetDirectionByRotation();
        }

        currentPlayerStatus.SetInvincible(true);

        currentPlayerMove.DisableInput();
        moveForward.enabled = false;

        currentPlayerMove.EnableInput();
        currentPlayerAnimator.SetTrigger("Run");
        ActivatePlayer(currentPlayerStatus);
        Debug.Log("플레이어 부활 및 무적 상태 설정");

        StartCoroutine(RemoveInvincibilityAfterDelay(2f)); //2초무적

        lastDeathType = DeathType.None;
    }

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
}