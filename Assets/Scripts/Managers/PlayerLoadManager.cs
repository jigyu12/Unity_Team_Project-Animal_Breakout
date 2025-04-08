using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerLoadManager
{

    // private Dictionary<int, GameObject> loadedCharacters = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> loadedCharacters = new();

    // 캐릭터 모델 미리 로드
    public void PreloadCharacterModels(List<int> animalIDs, UnityAction onAllLoaded = null)
    {
        int totalToLoad = animalIDs.Count;
        int loadedCount = 0;

        foreach (int animalID in animalIDs)
        {
            LoadCharacterModel(animalID, (status) =>
            {
                loadedCount++;
                if (loadedCount >= totalToLoad)
                {
                    onAllLoaded?.Invoke();
                }
            });
        }
    }

    // 캐릭터 모델 로드 (비동기)
    public void LoadCharacterModel(int animalID, UnityAction<PlayerStatus> onLoaded = null)
    {
        Addressables.LoadAssetAsync<GameObject>(animalID.ToString()).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject characterPrefab = handle.Result;
                loadedCharacters[animalID] = characterPrefab;

                PlayerStatus status = characterPrefab.GetComponent<PlayerStatus>();

                if (status != null && status.statData != null)
                {
                    Debug.Log($"Loaded pre-configured character: {status.statData.StringID}");
                    onLoaded?.Invoke(status);
                }
                else
                {
                    Debug.LogError($"Character prefab for ID {animalID} does not contain pre-configured stat data.");
                    return;
                }
            }
            else
            {
                Debug.LogError($"Failed to load character prefab for ID {animalID}");
            }
        };
    }



    // 캐릭터 프리팹 가져오기
    public GameObject GetLoadedCharacterPrefab(int animalID)
    {
        if (loadedCharacters.TryGetValue(animalID, out GameObject characterPrefab))
        {
            return characterPrefab;
        }
        return null;
    }
}
