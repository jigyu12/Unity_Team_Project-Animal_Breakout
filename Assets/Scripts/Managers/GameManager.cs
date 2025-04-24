// using UnityEngine;
// using UnityEngine.Events;
// using System.Collections;
// using TMPro;
// using UnityEngine.SceneManagement;
// public class GameManager : MonoBehaviour
// {
//    public UnityAction<PlayerStatus> onPlayerSpawned;
//    public UnityAction<PlayerStatus> onPlayerDied;
//    public UnityAction onGameOver;
//    //private RelayRunManager relayRunManager;

//    private void Start()
//    {

//    }

//    public void OnPlayerLoaded(PlayerStatus status)
//    {
//        Debug.Log($"Player Spawned: {status.name}");
//        onPlayerSpawned?.Invoke(status);
//    }



//    private void DeactivatePlayer(PlayerStatus playerStatus)
//    {
//        MoveForward moveComponent = playerStatus.GetComponent<MoveForward>();
//        if (moveComponent != null)
//        {
//            moveComponent.enabled = false;
//            Debug.Log($"MoveForward disabled for: {playerStatus.name}");
//        }
//    }

//    public void OnPlayerDied(PlayerStatus status)
//    {
//        Debug.Log($"Player Died: {status.name}");
//        onPlayerDied?.Invoke(status);
//        DeactivatePlayer(status);
//        var b = relayRunManager.HasNextRunner();
//        if (b)
//        {
//            RelayContinueUI relayContinueUI = FindObjectOfType<RelayContinueUI>();
//            if (relayContinueUI != null)
//            {
//                relayContinueUI.Show();
//            }
//            else
//            {
//                Debug.LogError("RelayContinueUI를 찾을 수 없습니다!");
//            }
//        }
//        else
//        {
//            GameOver();
//        }
//    }
//    public void StopAllMovements()
//    {
//        MoveForward[] movingObjects = FindObjectsOfType<MoveForward>();

//        foreach (var move in movingObjects)
//        {
//            move.enabled = false;
//        }
//    }

//    public void LoadScene(string sceneName)
//    {
//        SceneManager.LoadScene(sceneName);
//    }

//    public void GameOver()
//    {
//        Debug.Log("Game Over!");
//        onGameOver?.Invoke();
//    }
//    public IEnumerator ResumeWithCountdown(TMP_Text countdownText, GameObject pausePanel)
//    {
//        Time.timeScale = 0;
//        pausePanel.SetActive(false);
//        countdownText.gameObject.SetActive(true);

//        for (int i = 3; i > 0; i--)
//        {
//            countdownText.text = i.ToString();
//            yield return new WaitForSecondsRealtime(1);
//        }

//        countdownText.gameObject.SetActive(false);
//        Time.timeScale = 1;
//    }

// }
