using UnityEngine;
using UnityEngine.UI;

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
        SceneManagerEx.Instance.LoadScene("RunCopy");
    }
}