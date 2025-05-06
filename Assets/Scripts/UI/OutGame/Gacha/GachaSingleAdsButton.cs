using UnityEngine;

public class GachaSingleAdsButton : GachaButton
{
    private int adsRemainCount;
    
    protected void Awake()
    {
        adsRemainCount = 1;
        
        AlertPanelConfirmButtonFuncFactory.onGachaByAds += SetGachaButtonText;
    }

    protected void OnDestroy()
    {
        AlertPanelConfirmButtonFuncFactory.onGachaByAds -= SetGachaButtonText;
    }

    protected override void Start()
    {
        base.Start();
        
        this.headerText.text = "일일 무료";
        this.countText.text = $"광고 {adsRemainCount}/1";
    }
    
    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGachaByAds));
    }
    
    protected void SetGachaButtonText()
    {
        --adsRemainCount;

        if (adsRemainCount < 0)
        {
            adsRemainCount = 0;
            
            Debug.Assert(false, "Invalid adsRemainCount");
        }

        if (adsRemainCount == 0)
        {
            gachaButton.interactable = false;
        }
        
        this.countText.text = $"광고 {adsRemainCount}/1";
    }
}