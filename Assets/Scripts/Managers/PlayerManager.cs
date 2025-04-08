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

    [SerializeField]
    private int animalID = 100301;


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
        PlayDeathAnimation(status);
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

    private void PlayDeathAnimation(PlayerStatus playerStatus)
    {
        Animator animator = playerStatus.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Die");
            Debug.Log($"Death animation triggered for: {playerStatus.name}");
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
}
