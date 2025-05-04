public class GachaTenTimesButton : GachaButton
{
    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoTenTimesGacha));
    }
}