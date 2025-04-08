using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private int animalID = 100301;

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

    public void ContinuePlayer()
    {
        if (currentPlayerStatus == null)
        {
            Debug.LogError("부활할 플레이어가 없습니다.");
            return;
        }
        // 부활 시 무적 상태 설정
        currentPlayerStatus.SetInvincible(true);
        currentPlayerMove.EnableInput();
        currentPlayerAnimator.SetTrigger("Run");
        ActivatePlayer(currentPlayerStatus);
        Debug.Log("플레이어 부활 및 무적 상태 설정");

        StartCoroutine(RemoveInvincibilityAfterDelay(2f)); //2초무적
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


}
