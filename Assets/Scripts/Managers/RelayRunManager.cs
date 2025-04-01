using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelayRunManager : MonoBehaviour
{

    [SerializeField] private List<int> runnerIDs;
    private int currentRunnerIndex = 0;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        // 미리 모든 캐릭터 로드
        LoadManager.Instance.PreloadCharacterModels(runnerIDs, () =>
        {
            Debug.Log("All Runners Preloaded.");
            gameManager.LoadFirstRunner();
        });
    }

    public int GetNextRunnerID()
    {
        if (currentRunnerIndex >= runnerIDs.Count)
            return -1;

        int nextID = runnerIDs[currentRunnerIndex];
        currentRunnerIndex++;
        return nextID;
    }
    public void LoadNextRunner()
    {
        int nextID = GetNextRunnerID();
        if (nextID == -1)
        {
            gameManager.GameOver();
            return;
        }


        LoadManager.Instance.ActivateCharacter(nextID);
        PlayerStatus nextPlayerStatus = LoadManager.Instance.GetPlayerStatus(nextID);


        if (nextPlayerStatus != null)
        {
            nextPlayerStatus.SetInvincible(true);
            Debug.Log($"다음 주자 {nextID} 무적 상태 활성화");

            StartCoroutine(RemoveInvincibilityAfterDelay(nextPlayerStatus, 1f));
        }

        gameManager.OnPlayerLoaded(nextPlayerStatus);
    }

    private IEnumerator RemoveInvincibilityAfterDelay(PlayerStatus playerStatus, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerStatus != null)
        {
            playerStatus.SetInvincible(false);  // 무적 상태 OFF
            Debug.Log("무적 상태 해제됨");
        }
    }


    public bool HasNextRunner()
    {
        return currentRunnerIndex < runnerIDs.Count;
    }
}


// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;
// using System;

// public class RelayRunManager : MonoBehaviour
// {
//     public static RelayRunManager Instance { get; private set; }

//     [SerializeField] private List<int> runnerIDs;
//     private int currentRunnerIndex = 0;
//     private LoadManager loadManager;

//     public Action<PlayerStatus> onLoadPlayer;
//     public Action<PlayerStatus> onDiePlayer;

//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     void Start()
//     {
//         loadManager = FindObjectOfType<LoadManager>();
//         LoadFirstRunner();
//     }

//     public void LoadFirstRunner()
//     {
//         int nextID = runnerIDs[currentRunnerIndex];
//         currentRunnerIndex++;

//         loadManager.LoadCharacterModel(nextID, (PlayerStatus status) =>
//         {
//             status.transform.position = loadManager.transform.position;
//             StartCoroutine(ApplyInvincibility(status));

//             onLoadPlayer?.Invoke(status);
//             status.onDie = onDiePlayer;
//         });
//     }

//     public void LoadNextRunner()
//     {
//         if (currentRunnerIndex >= runnerIDs.Count)
//         {
//             GameManager.Instance.GameOver();
//             return;
//         }

//         int nextID = runnerIDs[currentRunnerIndex];
//         currentRunnerIndex++;

//         loadManager.LoadCharacterModel(nextID, (PlayerStatus status) =>
//         {
//             Vector3 spawnPos = loadManager.transform.position - loadManager.transform.forward * 3f;
//             status.transform.localPosition = new Vector3(spawnPos.x, 0, 0);
//             StartCoroutine(ApplyInvincibility(status));

//             onLoadPlayer?.Invoke(status);
//             status.onDie = onDiePlayer;
//         });
//     }

//     IEnumerator ApplyInvincibility(PlayerStatus status)
//     {
//         status.SetInvincible(true);
//         yield return new WaitForSeconds(1f);
//         status.SetInvincible(false);
//     }
//     public bool HasNextRunner()
//     {
//         return currentRunnerIndex < runnerIDs.Count;
//     }

//     public bool IsLastChance()
//     {
//         return currentRunnerIndex + 1 == runnerIDs.Count;
//     }

// }
