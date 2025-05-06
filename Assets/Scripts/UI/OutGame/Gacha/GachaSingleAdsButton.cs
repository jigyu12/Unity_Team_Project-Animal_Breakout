public class GachaSingleAdsButton : GachaButton
{
    protected override void Start()
    {
        base.Start();
        
        SetGachaButtonText("일일 무료", 1);
    }
    
    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGachaByAds));
    }
    
    protected void SetGachaButtonText(string headerText, int adsRemainCount)
    {
        this.headerText.text = headerText;
        this.countText.text = $"광고 {adsRemainCount}/1";
    }
}