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
