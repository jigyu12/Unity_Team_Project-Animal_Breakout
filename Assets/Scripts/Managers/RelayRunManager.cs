//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class RelayRunManager : MonoBehaviour
//{
//    [SerializeField] private List<int> runnerIDs;
//    private int currentRunnerIndex = 0;
//    private GameManager gameManager;
//    private GameSceneManager gameSceneManager;

//    private void Awake()
//    {
//        gameManager = FindObjectOfType<GameManager>();
//        runnerIDs = GameDataManager.Instance.GetRunnerIDs();
//    }

//    public int GetNextRunnerID()
//    {
//        if (currentRunnerIndex >= runnerIDs.Count)
//            return -1;

//        int nextID = runnerIDs[currentRunnerIndex];
//        currentRunnerIndex++;
//        return nextID;
//    }

//    public void LoadNextRunner()
//    {

//        int nextID = GetNextRunnerID();
//        if (nextID == -1)
//        {
//            gameManager.GameOver();
//            return;
//        }

//        Transform playerParent = GameObject.FindGameObjectWithTag("PlayerParent").transform;
//        GameObject prefab = LoadManager.Instance.GetCharacterPrefab(nextID);
//        if (prefab != null)
//        {
//            GameObject character = Instantiate(prefab, playerParent);
//            character.SetActive(true);

//            PlayerStatus playerStatus = character.GetComponent<PlayerStatus>();
//            if (playerStatus != null)
//            {
//                gameManager.OnPlayerLoaded(playerStatus);
//                GameObject.FindObjectOfType<GameSceneManager>().ActivatePlayer(playerStatus);
//            }
//            else
//            {
//                Debug.LogError($"Failed to get PlayerStatus on instantiated character for ID {nextID}");
//            }
//        }
//    }

//    public bool HasNextRunner()
//    {
//        return currentRunnerIndex < runnerIDs.Count;
//    }
//}
