using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;

    private readonly WaitForSeconds waitTime = new(Utils.GameStartWaitTime);
    
    public static event Action onGameStartButtonClicked;

    private void Awake()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager += OnSetStartAnimalIDInGameDataManagerHandler;
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager -= OnSetStartAnimalIDInGameDataManagerHandler;
    }

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        
        var gameDataManagerStartAnimalId = GameDataManager.Instance.StartAnimalID;
        if (gameDataManagerStartAnimalId == 0 || !IsContainsAnimalIdInTable(gameDataManagerStartAnimalId))
        {
            gameStartButton.interactable = false;
        }
        else
        {
            gameStartButton.interactable = true;
        }

        gameStartButton.onClick.AddListener(() =>
        {
            gameStartButton.interactable = false;
            onGameStartButtonClicked?.Invoke();
            StartCoroutine(OnGameStartButtonClicked());
        }
            );
    }

    private IEnumerator OnGameStartButtonClicked()
    {
        yield return waitTime;

        SceneManager.LoadScene("Run_new");
    }

    private void OnSetStartAnimalIDInGameDataManagerHandler(int animalID)
    {
        if (animalID == 0 || !IsContainsAnimalIdInTable(animalID))
        {
            gameStartButton.interactable = false;

            Debug.Assert(false, "Invalid animal ID");
            
            return;
        }

        gameStartButton.interactable = true;
    }

    private bool IsContainsAnimalIdInTable(int animalID)
    {
        var animalIdList = DataTableManager.animalDataTable.GetAnimalIDs();

        return animalIdList.Contains(animalID);
    }
}