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
        }
        
        Debug.Assert(false, $"Cant find AlertPanelInfoData in AlertPanelInfoDataType: {type}");
        
        return null;
    }
}