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

                    alertPanelInfoData.description = "게임을 종료하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.QuitGame);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoSingleGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description = "1개 열쇠 사용하여 진행하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoSingleGachaByAds:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = "광고를 보고 진행하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoSingleGachaByAds);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.DoTenTimesGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = "10개 열쇠 사용하여 진행하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.DoTenTimesGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughKeyToDoSingleGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    GameObject.FindGameObjectWithTag("GachaSingleButton").TryGetComponent(out GachaSingleButton gachaSingleButton);
                    var lackKeyAndGold = gachaSingleButton.GetLackKeyAndGoldCount(1);
                    alertPanelInfoData.description = $"열쇠 {lackKeyAndGold.lackKey}가 부족합니다.\n{lackKeyAndGold.lackGold} 골드를 사용하여 열쇠를 구매하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoSingleGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughKeyToDoTenTimesGacha:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    GameObject.FindGameObjectWithTag("GachaTenTimesButton").TryGetComponent(out GachaTenTimesButton gachaTenTimesButton);
                    var lackKeyAndGold = gachaTenTimesButton.GetLackKeyAndGoldCount(10);
                    alertPanelInfoData.description = $"열쇠 {lackKeyAndGold.lackKey}가 부족합니다.\n{lackKeyAndGold.lackGold} 골드를 사용하여 열쇠를 구매하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoTenTimesGacha);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.NotEnoughGold:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description = "골드가 부족합니다.";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.TooManyStaminaToPurchaseStamina:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = "행동력은 999를 넘을 수 없습니다.\n행동력을 구매 할 수 없습니다.";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.CheckStaminaPurchase:
                {
                    AlertPanelInfoData alertPanelInfoData = new();

                    alertPanelInfoData.description = "구매하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.CheckStaminaPurchase);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanel);
                    
                    return alertPanelInfoData;
                }
            case AlertPanelInfoDataType.EnforceAnimal:
                {
                    AlertPanelInfoData alertPanelInfoData = new();
                    
                    alertPanelInfoData.description = $"{GameDataManager.Instance.requiredTokenType.ToString()} {GameDataManager.Instance.requiredTokenCount}개와 코인 {GameDataManager.Instance.requiredGoldCount}개를 소모하여 강화를 진행하시겠습니까?";
                    alertPanelInfoData.confirmButtonAction = AlertPanelConfirmButtonFuncFactory.GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType.EnforceAnimal);
                    alertPanelInfoData.cancelButtonAction = AlertPanelCancelButtonFuncFactory.GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType.CloseAlertPanelBySetActive);
                    
                    return alertPanelInfoData;
                }
        }
        
        Debug.Assert(false, $"Cant find AlertPanelInfoData in AlertPanelInfoDataType: {type}");
        
        return null;
    }
}