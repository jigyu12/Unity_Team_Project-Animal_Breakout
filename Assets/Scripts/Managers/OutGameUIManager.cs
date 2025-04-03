using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class OutGameUIManager : MonoBehaviour
{
    public TMP_Text totalCoinText;
    public TMP_Text maxScoreText;
    public Button gameStartButton;
    private LoadingSceneManager loadingManager;

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        GameDataManager.Instance.Initialize();
    }

    public void SetTotalCoinText(long coin)
    {
        totalCoinText.text = $"Total Coins : {coin.ToString()}";
    }

    public void SetMaxScoreText(long score)
    {
        maxScoreText.text = $"Max Score : {score.ToString()}";
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
            runnerIDs.Add(animal.AnimalID); // Id없어서 AnimalId로 햇음
        }
        GameDataManager.Instance.SetRunnerIDs(runnerIDs);

        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }


}


