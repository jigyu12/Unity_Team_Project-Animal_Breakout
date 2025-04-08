using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyPanel : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    
    private readonly WaitForSeconds waitTime = new(Utils.GameStartWaitTime);
    
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
        
        SceneManager.LoadScene("Run");
    }
}