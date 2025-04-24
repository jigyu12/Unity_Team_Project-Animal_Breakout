using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : UIElement
{
    [SerializeField] private Button pauseButton;


    public override void Initialize()
    {
        base.Initialize();

        pauseButton.onClick.RemoveAllListeners();
        pauseButton.onClick.AddListener(() => gameUIManager.Pause());
    }

    public void SetInteractable(bool interactable)
    {
        pauseButton.interactable = interactable;
    }
}
