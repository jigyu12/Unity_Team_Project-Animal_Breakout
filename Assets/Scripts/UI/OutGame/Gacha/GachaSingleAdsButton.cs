using UnityEngine;

public class GachaSingleAdsButton : GachaButton
{
    public bool IsAdsRemain
    {
        get => GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount > 0;
    }

    protected void Awake()
    {
        AlertPanelConfirmButtonFuncFactory.onGachaByAds += SetGachaButtonText;
    }

    protected void OnDestroy()
    {
        AlertPanelConfirmButtonFuncFactory.onGachaByAds -= SetGachaButtonText;
    }

    protected override void Start()
    {
        base.Start();

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
        this.countText.text = LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.GachaSingleAdsStringKey
            , GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount, GameDataManager.Instance.PlayerAccountData.GachaSingleAdsCount);
    }
}