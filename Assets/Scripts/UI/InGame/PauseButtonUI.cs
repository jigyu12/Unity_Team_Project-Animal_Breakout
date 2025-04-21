using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    private GameUIManager gameUIManager;

    public void Initialize(GameUIManager uiManager)
    {
        gameUIManager = uiManager;

        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(() => gameUIManager.Pause());
    }

    public void SetInteractable(bool interactable)
    {
        pauseButton.interactable = interactable;
    }
}
