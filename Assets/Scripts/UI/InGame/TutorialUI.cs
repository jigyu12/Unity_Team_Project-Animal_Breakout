using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
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

        if (GameDataManager.Instance.PlayerAccountData.IsTutorialComplete)
        {
            return;
        }

        tutorialButton.onClick.AddListener(SetTutorialImage);
        
        gameManager.AddGameStateExitAction(GameManager_new.GameState.WaitLoading, Show);
    }

    public override void Show()
    {
        base.Show();
        InitializeTutorialImage();

        gameObject.SetActive(true);
        gameManager.SetTimeScale(0f);
        SetTutorialImage();
    }

    private void SetTutorialImage()
    {
        if (tutorialIndex >= tutorialImages.Count)
        {
            gameManager.SetTimeScale(1f);
            gameObject.SetActive(false);
            GameDataManager.Instance.PlayerAccountData.OnTutorialComplete();
            return;
        }

        tutorialImage.sprite = tutorialImages[tutorialIndex];
        tutorialIndex++;
    }

    private void InitializeTutorialImage()
    {
        tutorialImages.Clear();
        for (int i = 1; i <= 6; i++)
        {
            Sprite tutorialSprite = LocalizationUtility.GetLocalizeSprite(LocalizationUtility.defaultSpriteTableName, "Tutorial0" + i.ToString());
            tutorialImages.Add(tutorialSprite);
        }
    }
}
