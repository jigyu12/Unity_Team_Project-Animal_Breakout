using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : UIElement
{
    [SerializeField] private Button pauseButton;

    
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
