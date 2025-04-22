using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;

    private readonly WaitForSeconds waitTime = new(Utils.GameStartWaitTime);
    private readonly int staminaRequiredToStartGame = 5;
    private bool isStaminaEnoughToStartGame;
    
    public static event Action<int> onGameStartButtonClicked;

    private void Awake()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager += OnSetStartAnimalIDInGameDataManagerHandler;
        GameDataManager.onStaminaChangedInGameDataManager += OnStaminaChangedInGameDataManagerHandler;
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalIDInGameDataManager -= OnSetStartAnimalIDInGameDataManagerHandler;
        GameDataManager.onStaminaChangedInGameDataManager -= OnStaminaChangedInGameDataManagerHandler;
    }

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        
        var gameDataManagerStartAnimalId = GameDataManager.Instance.StartAnimalID;
        if (gameDataManagerStartAnimalId == 0 || !IsContainsAnimalIdInTable(gameDataManagerStartAnimalId) || !isStaminaEnoughToStartGame)
        {
            gameStartButton.interactable = false;
        }
        else
        {
            gameStartButton.interactable = true;
        }

        gameStartButton.onClick.AddListener(() =>
        {
            onGameStartButtonClicked?.Invoke(staminaRequiredToStartGame);
            gameStartButton.interactable = false;
            StartCoroutine(OnGameStartButtonClicked());
        }
            );
    }

    private IEnumerator OnGameStartButtonClicked()
    {
        yield return waitTime;

        SceneManager.LoadScene("Run");
    }

    private void OnSetStartAnimalIDInGameDataManagerHandler(int animalID, int currentStamina)
    {
        if (animalID == 0 || !IsContainsAnimalIdInTable(animalID))
        {
            gameStartButton.interactable = false;

            Debug.Assert(false, "Invalid animal ID");
            
            return;
        }

        if (currentStamina < staminaRequiredToStartGame)
        {
            gameStartButton.interactable = false;
            
            return;
        }

        gameStartButton.interactable = true;
    }

    private bool IsContainsAnimalIdInTable(int animalID)
    {
        var animalIdList = DataTableManager.animalDataTable.GetAnimalIDs();

        return animalIdList.Contains(animalID);
    }

    private void OnStaminaChangedInGameDataManagerHandler(int currentStamina, int maxStamina)
    {
        if (currentStamina >= staminaRequiredToStartGame)
        {
            gameStartButton.interactable = true;
            isStaminaEnoughToStartGame = true;
        }
        else
        {
            gameStartButton.interactable = false;
            isStaminaEnoughToStartGame = false;
        }
    }
}