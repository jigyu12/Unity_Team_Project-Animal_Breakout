using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;

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

    public void LoadScene(string sceneName)
    {
        SceneManagerEx.Instance.LoadScene(sceneName);
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        onGameOver?.Invoke();
    }
    public IEnumerator ResumeWithCountdown(TMP_Text countdownText, GameObject pausePanel)
    {
        Time.timeScale = 0;
        pausePanel.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
