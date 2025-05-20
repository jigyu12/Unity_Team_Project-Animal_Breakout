using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerLoadManager
{

    // private Dictionary<int, GameObject> loadedCharacters = new Dictionary<int, GameObject>();
    private Dictionary<string, GameObject> loadedCharacters = new();

    // 캐릭터 모델 미리 로드
    public void PreloadCharacterModels(List<string> prefabNames, UnityAction onAllLoaded = null)
    {
        int totalToLoad = prefabNames.Count;
        int loadedCount = 0;

        foreach (string prefabName in prefabNames)
        {
            LoadCharacterModel(prefabName, () =>
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
    public void LoadCharacterModel(string prefabName, UnityAction onLoaded = null)
    {
        Addressables.LoadAssetAsync<GameObject>(prefabName.ToString()).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject characterPrefab = handle.Result;
                loadedCharacters[prefabName] = characterPrefab;


                Debug.Log($"Loaded pre-configured character: {prefabName}");
                onLoaded?.Invoke();
            }
            else
            {
                Debug.LogError($"Failed to load character prefab for ID {prefabName}");
            }
        };
    }



    // 캐릭터 프리팹 가져오기
    public GameObject GetLoadedCharacterPrefab(string prefabName)
    {
        if (loadedCharacters.TryGetValue(prefabName, out GameObject characterPrefab))
        {
            return characterPrefab;
        }
        return null;
    }
}
