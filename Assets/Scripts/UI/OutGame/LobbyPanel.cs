using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    
    private readonly WaitForSeconds waitTime = new(1f);

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.interactable = true;
        
        gameStartButton.onClick.AddListener(() =>
        {
            gameStartButton.interactable = false;
            StartCoroutine(OnGameStartButtonClicked());
        }
            );
    }

    private IEnumerator OnGameStartButtonClicked()
    {
        AnimalDatabaseLoader loader = FindObjectOfType<AnimalDatabaseLoader>();
        if (loader != null)
        {
            loader.LoadDataFromCSV();
        }

        AnimalDatabase database = GameDataManager.Instance.GetAnimalDatabase();
        if (database == null)
        {
            Debug.LogError("AnimalDatabase를 찾을 수 없습니다!");
            yield break;
        }
        List<int> runnerIDs = new List<int>();
        foreach (AnimalStatus animal in database.Animals)
        {
            runnerIDs.Add(animal.AnimalID); // Id없어서 AnimalId로 햇음
        }
        GameDataManager.Instance.SetRunnerIDs(runnerIDs);

        yield return waitTime;
        
        // ✅ 로딩씬으로 전환
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }
}