using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;
using System.Collections.Generic;

public class LoadManager : Singleton<LoadManager>
{
    public Transform modelContainer;
    public AnimalDatabase database;
    public int currentAnimalID;

    public GameObject playerParent;
    private GameObject currentModel;
    private Dictionary<int, GameObject> loadedCharacters = new Dictionary<int, GameObject>();
    private int loadedCount = 0;   // 로드 완료된 캐릭터 수
    private int totalToLoad = 0;

    public UnityAction onAllLoaded;

    private void Start()
    {
        // 초기 로드 처리
    }
    public void PreloadCharacterModels(List<int> animalIDs, UnityAction onAllLoaded = null)
    {
        this.onAllLoaded = onAllLoaded;
        totalToLoad = animalIDs.Count;
        loadedCount = 0;

        foreach (int animalID in animalIDs)
        {
            LoadCharacterModel(animalID, OnCharacterLoaded);
        }
    }

    private void OnCharacterLoaded(PlayerStatus status)
    {
        loadedCount++;
        Debug.Log($"Character Loaded: {status.name} (Loaded {loadedCount}/{totalToLoad})");

        // 모든 로드 완료 시 호출
        if (loadedCount >= totalToLoad)
        {
            Debug.Log("All characters preloaded.");
            onAllLoaded?.Invoke();
        }
    }


    public void LoadCharacterModel(int animalID, UnityAction<PlayerStatus> onLoaded = null)
    {
        if (loadedCharacters.ContainsKey(animalID))
        {
            onLoaded?.Invoke(loadedCharacters[animalID].GetComponent<PlayerStatus>());
            return;
        }

        AnimalStatus character = database.GetAnimalByID(animalID);
        if (character == null)
        {
            Debug.LogError($"Animal ID {animalID} not found in database.");
            return;
        }

        Addressables.InstantiateAsync(character.PrefabKey).Completed += (handle) =>
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load character prefab: " + character.PrefabKey);
                return;
            }

            GameObject characterObject = handle.Result;
            characterObject.transform.SetParent(playerParent.transform, true);
            characterObject.SetActive(false);  // 비활성화로 대기

            loadedCharacters[animalID] = characterObject;

            var status = characterObject.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.Init(animalID, database);
                onLoaded?.Invoke(status);
            }

            Debug.Log($"Character Loaded: {status.name}");
        };
    }
    public void LoadCharacterModels(List<int> animalsID, UnityAction<PlayerStatus> onLoaded = null)
    {
        for (int i = 0; i < animalsID.Count; i++)
        {
            LoadCharacterModel(animalsID[i]);

        }

    }
    public void ActivateCharacter(int animalID)
    {
        if (loadedCharacters.ContainsKey(animalID))
        {
            if (currentModel != null)
            {
                currentModel.SetActive(false);
            }

            currentModel = loadedCharacters[animalID];
            currentModel.SetActive(true);

            var move = currentModel.GetComponent<PlayerMove>();
            if (move != null)
            {
                move.Initialize(FindObjectOfType<Lane>());
            }

            var animator = currentModel.GetComponent<Animator>();
            if (animator != null)
                GetComponent<Animator>().runtimeAnimatorController = animator.runtimeAnimatorController;

            Debug.Log($"Character Activated: {currentModel.name}");
        }
        else
        {
            Debug.LogError($"Character with ID {animalID} not found in loaded characters.");
        }
    }

    public PlayerStatus GetPlayerStatus(int animalID)
    {
        if (loadedCharacters.ContainsKey(animalID))
        {
            return loadedCharacters[animalID].GetComponent<PlayerStatus>();
        }
        return null;
    }

}


// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;

// public class LoadManager : MonoBehaviour
// {
//     public Transform modelContainer;
//     public AnimalDatabase database;
//     public int currentAnimalID;

//     public GameObject playerParent;
//     private GameObject currentModel;

//     private void Start()
//     {
//         //  LoadCharacterModel(currentAnimalID);
//     }

//     public void LoadCharacterModel(int animalID, System.Action<PlayerStatus> onLoaded = null)
//     {
//         AnimalStatus character = database.GetAnimalByID(animalID);
//         if (character == null) return;

//         currentAnimalID = animalID;
//         PlayerPrefs.SetInt("SelectedAnimalID", currentAnimalID);

//         if (currentModel != null)
//         {
//             Addressables.ReleaseInstance(currentModel);
//         }

//         Addressables.InstantiateAsync(character.PrefabKey).Completed += (handle) =>
//         {
//             if (handle.Status != AsyncOperationStatus.Succeeded) return;

//             currentModel = handle.Result;
//             currentModel.transform.SetParent(playerParent.transform, true);
//             var status = currentModel.GetComponent<PlayerStatus>();
//             if (status != null)
//             {
//                 status.Init(animalID, database);
//                 onLoaded?.Invoke(status);
//             }

//             var move = currentModel.GetComponent<PlayerMove>();
//             if (move != null)
//             {
//                 move.Initialize(
//                     FindObjectOfType<Lane>()
//                 );
//             }

//             var attacker = currentModel.GetComponent<Attacker>();
//             if (attacker != null && attacker.powerDisplay != null)
//             {
//                 attacker.powerDisplay.Init(attacker);
//             }

//             var animator = currentModel.GetComponent<Animator>();
//             if (animator != null)
//                 GetComponent<Animator>().runtimeAnimatorController = animator.runtimeAnimatorController;
//         };
//     }

// }
