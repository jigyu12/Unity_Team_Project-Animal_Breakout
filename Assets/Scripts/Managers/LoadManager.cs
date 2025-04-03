using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LoadManager : Singleton<LoadManager>
{
    private PlayerLoadManager playerLoadManager;

    private void Awake()
    {
        InitializePlayerLoadManager();
    }

    private void InitializePlayerLoadManager()
    {
        if (playerLoadManager == null)
        {
            playerLoadManager = FindObjectOfType<PlayerLoadManager>();
            if (playerLoadManager == null)
            {
                GameObject obj = new GameObject("PlayerLoadManager");
                playerLoadManager = obj.AddComponent<PlayerLoadManager>();
                DontDestroyOnLoad(obj);
            }
        }
    }

    public void PreloadCharacters(List<int> animalIDs, UnityAction onAllLoaded = null)
    {
        if (playerLoadManager != null)
        {
            playerLoadManager.PreloadCharacterModels(animalIDs, onAllLoaded);
        }
    }

    public GameObject GetCharacterPrefab(int animalID)
    {
        return playerLoadManager.GetLoadedCharacterPrefab(animalID);
    }
}
