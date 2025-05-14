using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : UIElement
{
    [SerializeField] private Button pauseButton;


    public override void Initialize()
    {
        base.Initialize();

        pauseButton.onClick.RemoveAllListeners();
        // pauseButton.onClick.AddListener(() => gameUIManager.Pause());
        pauseButton.onClick.AddListener(() =>
   {
       gameUIManager.Pause();

       SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
       SoundManager.Instance.StopBgm();
       //    SoundManager.Instance.StopSfx();
   });
    }

    public void SetInteractable(bool interactable)
    {
        pauseButton.interactable = interactable;
    }
}
