using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnGameStartButtonClicked()
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
            return;
        }
        List<int> runnerIDs = new List<int>();
        foreach (AnimalStatus animal in database.Animals)
        {
            runnerIDs.Add(animal.AnimalID);
        }
        GameDataManager.Instance.SetRunnerIDs(runnerIDs);
        SceneManager.LoadScene("RunMinjae");
    }

}