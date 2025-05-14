using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class RealGiveUpPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject RealGiveUpPanel;
    [SerializeField] private GameObject GameResultPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    private void Start()
    {

        noButton.onClick.RemoveAllListeners();
        // noButton.onClick.AddListener(OnNoButtonClicked);

        noButton.onClick.AddListener(() =>
        {
            OnNoButtonClicked();

            SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
        });
        yesButton.onClick.RemoveAllListeners();
        // yesButton.onClick.AddListener(OnYesButtonClicked);
        yesButton.onClick.AddListener(() =>
    {
        OnYesButtonClicked();

        SoundManager.Instance.PlaySfx(SfxClipId.ButtonTouch);
    });

    }
    private void OnNoButtonClicked()
    {
        RealGiveUpPanel.SetActive(false);
    }
    private void OnYesButtonClicked()
    {
        GameResultPanel.SetActive(true);
        RealGiveUpPanel.SetActive(false);

    }
}
