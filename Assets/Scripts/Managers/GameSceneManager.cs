using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    private GameManager gameManager;
    private RelayRunManager relayRunManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 씬 로드될 때마다 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Run") || scene.name.Contains("InGame"))
        {
            StartGameScene();
        }
    }

    // 게임 씬 시작
    public void StartGameScene()
    {
        gameManager = FindObjectOfType<GameManager>();
        relayRunManager = FindObjectOfType<RelayRunManager>();

        if (gameManager == null || relayRunManager == null)
        {
            Debug.LogError("GameManager or RelayRunManager not found!");
            return;
        }

        Debug.Log("Starting Game Scene...");
        StartCoroutine(LoadGameResources());
    }

    private IEnumerator LoadGameResources()
    {
        // 로딩창 Additive로 띄우기
        yield return SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        // 로딩 시작
        bool isLoadingComplete = false;
        LoadManager.Instance.InitializeGame(() =>
        {
            isLoadingComplete = true;
        });

        // 로딩 완료 대기
        while (!isLoadingComplete)
        {
            yield return null;
        }

        Debug.Log("Game Resources Loaded!");

        // 로딩 완료 후 로딩창 제거
        yield return SceneManager.UnloadSceneAsync("LoadingScene");

        // 로딩 완료 후 첫 러너 생성
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Game Started!");
        LoadFirstRunner();
    }

    private void LoadFirstRunner()
    {
        int nextID = relayRunManager.GetNextRunnerID();
        if (nextID == -1)
        {
            Debug.LogError("No next runner ID available.");
            return;
        }

        Transform playerParent = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        GameObject prefab = LoadManager.Instance.GetCharacterPrefab(nextID);
        if (prefab != null)
        {
            GameObject character = Instantiate(prefab, playerParent);
            character.SetActive(true);

            PlayerStatus playerStatus = character.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                gameManager.OnPlayerLoaded(playerStatus);
                gameManager.ActivatePlayer(playerStatus);
                Debug.Log($"Player {nextID} spawned successfully.");
            }
            else
            {
                Debug.LogError("PlayerStatus component not found on instantiated character.");
            }
        }
        else
        {
            Debug.LogError($"Character prefab not found for ID {nextID}.");
        }
    }

    public void OnGameEnd()
    {
        Debug.Log("Game Ended. Returning to Title...");
        SceneManager.LoadScene("MainTitleScene", LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
