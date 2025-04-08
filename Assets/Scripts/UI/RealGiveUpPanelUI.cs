using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class RealGiveUpPanelUI : MonoBehaviour
{
    public GameObject RealGiveUpPanel;
    public GameObject GameResultPanel;
    public Button yesButton;
    public Button noButton;
    private void Start()
    {

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(OnNoButtonClicked);

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesButtonClicked);

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
