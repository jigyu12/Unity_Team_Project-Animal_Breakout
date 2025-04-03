using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerLoadManager : MonoBehaviour
{
    private Dictionary<int, GameObject> loadedCharacters = new Dictionary<int, GameObject>();

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

    public void LoadCharacterModel(int animalID, UnityAction<PlayerStatus> onLoaded = null)
    {
        AnimalDatabase database = GameDataManager.Instance.GetAnimalDatabase();
        if (database == null)
        {
            Debug.LogError("AnimalDatabase not found.");
            return;
        }

        AnimalStatus character = database.GetAnimalByID(animalID);
        if (character == null)
        {
            Debug.LogError($"Character data not found for ID {animalID}");
            return;
        }

        Addressables.LoadAssetAsync<GameObject>(character.PrefabKey).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject characterPrefab = handle.Result;
                loadedCharacters[animalID] = characterPrefab;
                //  onLoaded?.Invoke(null);
                PlayerStatus status = characterPrefab.GetComponent<PlayerStatus>();

                if (status == null)
                {
                    status = characterPrefab.AddComponent<PlayerStatus>();
                    status.Init(animalID, database); // Init으로 데이터 세팅
                }
                onLoaded?.Invoke(status);
                Debug.Log($"Character model loaded for ID {animalID}");
            }
            else
            {
                Debug.LogError($"Failed to load character prefab for ID {animalID}");
            }
        };
    }

    public GameObject GetLoadedCharacterPrefab(int animalID)
    {
        if (loadedCharacters.TryGetValue(animalID, out GameObject characterPrefab))
        {
            return characterPrefab;
        }
        return null;
    }
}
