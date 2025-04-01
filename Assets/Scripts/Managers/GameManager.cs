using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityAction<PlayerStatus> onPlayerSpawned;
    public UnityAction<PlayerStatus> onPlayerDied;
    public UnityAction onGameOver;
    private RelayRunManager relayRunManager;

    private void Awake()
    {
        relayRunManager = FindObjectOfType<RelayRunManager>();
    }

    private void Start()
    {
        // LoadFirstRunner();
    }

    public void LoadFirstRunner()
    {
        int nextID = relayRunManager.GetNextRunnerID();
        if (nextID == -1)
        {
            GameOver();
            return;
        }

        LoadManager.Instance.ActivateCharacter(nextID);  // SetActive로 활성화
        OnPlayerLoaded(LoadManager.Instance.GetPlayerStatus(nextID));
    }

    public void OnPlayerLoaded(PlayerStatus status)
    {
        Debug.Log($"Player Spawned: {status.name}");
        onPlayerSpawned?.Invoke(status);
    }

    public void OnPlayerDied(PlayerStatus status)
    {
        Debug.Log($"Player Died: {status.name}");
        onPlayerDied?.Invoke(status);

        if (relayRunManager.HasNextRunner())
        {
            RelayContinueUI relayContinueUI = FindObjectOfType<RelayContinueUI>();
            if (relayContinueUI != null)
            {
                relayContinueUI.Show();
            }
            else
            {
                Debug.LogError("RelayContinueUI를 찾을 수 없습니다!");
            }
        }
        else
        {
            GameOver();
        }
    }
    public void StopAllMovements()
    {
        MoveForward[] movingObjects = FindObjectsOfType<MoveForward>();

        foreach (var move in movingObjects)
        {
            move.enabled = false;
        }
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        onGameOver?.Invoke();
    }
}
