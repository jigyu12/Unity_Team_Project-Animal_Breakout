using System.Collections;
using UnityEngine;

public class PlayerManager : InGameManager
{
    public GameObject playerRoot;

    private PlayerRotator playerRotator;

    [ReadOnly]
    public PlayerStatus currentPlayerStatus;
    [ReadOnly]
    public PlayerMove currentPlayerMove;

    private int animalID = 0;//100301;

    private void Awake()
    {
        playerRotator = GetComponent<PlayerRotator>();
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.AddGameStateEnterAction(GameManager_new.GameState.GameOver, () => DisablePlayer(currentPlayerStatus));

    }
    public void SetPlayer()
    {
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
        PlayerMove move = playerStatus.GetComponent<PlayerMove>();
        if (move != null)
        {
            move.DisableInput();
            Debug.Log($"Player movement disabled for: {playerStatus.name}");
        }

        MoveForward moveForward = playerStatus.GetComponent<MoveForward>();
        if (moveForward != null)
        {
            moveForward.enabled = false;
            Debug.Log($"MoveForward disabled for: {playerStatus.name}");
        }
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
        GameManager.OnGameOver();
        //  Destroy(playerStatus.gameObject);
        // Debug.Log($"Player {playerStatus.name} destroyed.");
    }
    
    public void SetStartAnimalID(int id)
    {
        Debug.Log($"Set Animal ID In PlayerManager: {id}");
        
        animalID = id;
    }
}
