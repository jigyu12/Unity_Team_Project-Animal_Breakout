using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RelayRunManager : MonoBehaviour
{
    public static RelayRunManager Instance { get; private set; }

    [SerializeField] private List<int> runnerIDs;
    private int currentRunnerIndex = 0;
    private PlayerManager playerManager;

    [SerializeField]
    private RoadManager roadManager;
    [SerializeField]
    private RoadChunkRotator roadChunkRotator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        LoadFirstRunner();
    }

    public void LoadFirstRunner()
    {
        int nextID = runnerIDs[currentRunnerIndex];
        currentRunnerIndex++;

        playerManager.LoadCharacterModel(nextID, (PlayerStatus1 status) =>
        {
            status.transform.position = playerManager.transform.position;
            StartCoroutine(ApplyInvincibility(status));

            roadManager.Initialize();
            roadChunkRotator.Initialize();
        });
    }

    public void LoadNextRunner()
    {
        if (currentRunnerIndex >= runnerIDs.Count)
        {
            GameManager.Instance.GameOver();
            return;
        }

        int nextID = runnerIDs[currentRunnerIndex];
        currentRunnerIndex++;

        playerManager.LoadCharacterModel(nextID, (PlayerStatus1 status) =>
        {
            Vector3 spawnPos = playerManager.transform.position - playerManager.transform.forward * 3f;
            status.transform.localPosition= new Vector3(spawnPos.x, 0, 0);
            StartCoroutine(ApplyInvincibility(status));
        });
    }

    IEnumerator ApplyInvincibility(PlayerStatus1 status)
    {
        status.SetInvincible(true);
        yield return new WaitForSeconds(1f);
        status.SetInvincible(false);
    }
    public bool HasNextRunner()
    {
        return currentRunnerIndex < runnerIDs.Count;
    }

    public bool IsLastChance()
    {
        return currentRunnerIndex + 1 == runnerIDs.Count;
    }

}
