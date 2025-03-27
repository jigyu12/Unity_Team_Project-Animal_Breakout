using UnityEngine;
using System.Collections.Generic;

public class RelayRunManager : MonoBehaviour
{
    [SerializeField] private List<int> runnerIDs;
    private int currentRunnerIndex = 0;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        LoadNextRunner();
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
        playerManager.LoadCharacterModel(nextID);
    }


}
