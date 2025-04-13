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
        GameDataManager.onSetStartAnimalID += OnSetStartAnimalIDHandler;
    }

    private void OnDestroy()
    {
        GameDataManager.onSetStartAnimalID -= OnSetStartAnimalIDHandler;
    }

    private void Start()
    {
        gameStartButton.onClick.RemoveAllListeners();
        if (GameDataManager.Instance.StartAnimalID == 0)
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

    private void OnSetStartAnimalIDHandler(int animalID)
    {
        if (animalID == 0)
            return;

        gameStartButton.interactable = true;
    }
}