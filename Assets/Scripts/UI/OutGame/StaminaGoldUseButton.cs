using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGoldUseButton : MonoBehaviour
{
    [SerializeField] private Button staminaGoldUseButton;
    [SerializeField] private TMP_Text staminaGoldUseText;
    
    [SerializeField] private long staminaGoldUseCost;
    [SerializeField] private int staminaToAdd;
    
    public static event Action<long, int> onStaminaGoldUseButtonClicked;
    
    private OutGameUIManager outGameUIManager;

    private void Awake()
    {
        GameDataManager.onLocaleChange += SetStaminaGoldUseText;
    }

    private void OnDestroy()
    {
        GameDataManager.onLocaleChange -= SetStaminaGoldUseText;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        outGameUIManager = outGameManager.OutGameUIManager;
        
        staminaGoldUseButton.onClick.RemoveAllListeners();
        staminaGoldUseButton.onClick.AddListener(() =>
        {
            onStaminaGoldUseButtonClicked?.Invoke(staminaGoldUseCost, staminaToAdd);
            outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.CheckStaminaPurchase));
        });

        SetStaminaGoldUseText();
    }

    public void SetStaminaGoldUseText()
    {
        staminaGoldUseText.text = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.StaminaGoldUseStringKey, staminaGoldUseCost);
    }
}