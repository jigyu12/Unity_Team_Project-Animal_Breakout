using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : InGameManager
{
    public GameObject playerRoot;

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
    public void OnDie()
    {
        GameManager.OnGameOver();
    }
}
