using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIElement
{
    [SerializeField]
    private Button tutorialButton;

    [SerializeField]
    private Image tutorialImage;

    [SerializeField]
    private List<Sprite> tutorialImages = new();

    private int tutorialIndex = 0;

    public override void Initialize()
    {
        base.Initialize();

        if (GameDataManager.Instance.PlayerLevelSystem.IsTutorialComplete)
        {
            return;
        }

        tutorialButton.onClick.AddListener(SetTutorialImage);
        SetTutorialImage();
        gameManager.AddGameStateExitAction(GameManager_new.GameState.WaitLoading, Show);
    }

    public override void Show()
    {
        base.Show();
        gameObject.SetActive(true);
        gameManager.SetTimeScale(0f);
    }

    private void SetTutorialImage()
    {
        if(tutorialIndex>= tutorialImages.Count)
        {
            gameManager.SetTimeScale(1f);
            gameObject.SetActive(false);
            GameDataManager.Instance.PlayerLevelSystem.OnTutorialComplete();
            return;
        }

        tutorialImage.sprite = tutorialImages[tutorialIndex];
        tutorialIndex++;
    }
}
