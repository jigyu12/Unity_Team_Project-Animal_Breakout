using UnityEngine;

public static class AlertPanelInfoDataFactory
{
    public static AlertPanelInfoData GetAlertPanelInfoData(AlertPanelInfoDataType type)
    {
        switch (type)
        {
            case AlertPanelInfoDataType.TestCloseAlertPanelSingle:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = "Test Close AlertPanel Single";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.TestCloseAlertPanelDouble:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = "Test Close AlertPanel Double";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.QuitGame:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalQuitGameStringKey);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.QuitGame);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoSingleGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description = alertPanelInfoData.description = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTicketStringKey, 1);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoSingleGachaByAds:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalAdsStringKey);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleGachaByAds);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoTenTimesGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTicketStringKey, 10);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoTenTimesGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughKeyToDoSingleGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    GameObject.FindGameObjectWithTag("GachaSingleButton").TryGetComponent(out GachaSingleButton gachaSingleButton);
                    var lackKeyAndGold = gachaSingleButton.GetLackKeyAndGoldCount(1);
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTicketBuyStringKey, lackKeyAndGold.lackKey, lackKeyAndGold.lackGold);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoSingleGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughKeyToDoTenTimesGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    GameObject.FindGameObjectWithTag("GachaTenTimesButton").TryGetComponent(out GachaTenTimesButton gachaTenTimesButton);
                    var lackKeyAndGold = gachaTenTimesButton.GetLackKeyAndGoldCount(10);
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTicketBuyStringKey, lackKeyAndGold.lackKey, lackKeyAndGold.lackGold);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoTenTimesGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughGold:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalNoMoneyStringKey);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.TooManyStaminaToPurchaseStamina:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalManyStaminaStringKey);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.CheckStaminaPurchase:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalBuyStringKey);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CheckStaminaPurchase);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.EnforceAnimal:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    alertPanelInfoData.description =  LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalUpgradeProcessStringKey,
                        GameDataManager.Instance.requiredTokenType.ToString() ,GameDataManager.Instance.requiredTokenCount, GameDataManager.Instance.requiredGoldCount);
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.EnforceAnimal);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseEnforceAlertPanelBySetActive);
                    
                    return alertPanelInfoData;
                }
        }
        
        Debug.Assert(false, $"Cant find AlertPanelInfoData in AlertPanelInfoDataType: {type}");
        
        return null;
    }
}