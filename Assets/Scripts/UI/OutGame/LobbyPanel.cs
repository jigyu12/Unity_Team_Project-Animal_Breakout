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
        yield return waitTime;
        
        SceneManager.LoadScene("RunMinjae");
    }
}