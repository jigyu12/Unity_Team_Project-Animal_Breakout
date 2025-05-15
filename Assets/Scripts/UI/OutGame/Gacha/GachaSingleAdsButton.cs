public class GachaSingleAdsButton : GachaButton
{
    public bool IsAdsRemain
    {
        get => GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount > 0;
    }

    protected void Awake()
    {
        AlertPanelConfirmButtonFuncFactory.onGachaByAds += SetGachaButtonText;
        GameDataManager.onLocaleChange += SetGachaButtonText;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        AlertPanelConfirmButtonFuncFactory.onGachaByAds -= SetGachaButtonText;
        GameDataManager.onLocaleChange -= SetGachaButtonText;
    }

    protected override void Start()
    {
        base.Start();

        gachaButton.interactable = IsAdsRemain;

        SetGachaButtonText();
    }

    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGachaByAds));
    }

    protected void SetGachaButtonText()
    {
        this.countText.text = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.GachaSingleAdsStringKey
            , GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount, GameDataManager.Instance.PlayerAccountData.GachaSingleAdsCount);
    }
}