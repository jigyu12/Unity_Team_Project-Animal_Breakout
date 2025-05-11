using UnityEngine;

public class GachaSingleAdsButton : GachaButton
{
    //private int adsRemainCount;
    //UI에 정보나 기능이 있으면 안됩니다

    public bool IsAdsRemain
    {
        get => GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount > 0;
    }

    protected void Awake()
    {
        //adsRemainCount = 1;

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
        SetGachaButtonText();
        gachaButton.interactable = IsAdsRemain;
    }

    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGachaByAds));
        GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount--;
        
        gachaButton.interactable = IsAdsRemain;
    }

    protected void SetGachaButtonText()
    {
        if (GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount < 0)
        {
            GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount = 0;
            Debug.Assert(false, "Invalid adsRemainCount");
        }

        this.countText.text = $"광고 {GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount}/{GameDataManager.Instance.PlayerAccountData.GachaSingleAdsCount}";
    }
}