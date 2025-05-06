using UnityEngine;
using UnityEngine.UI;

public class GachaTenTimesButton : GachaButton
{
    [SerializeField] private Image keyImage;
    
    protected override void Start()
    {
        base.Start();
        
        SetGachaButtonText("10회", "10");
    }

    public override void DoGacha()
    {
        if (GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey >= 10)
        {
            outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoTenTimesGacha));
        }
        else
        {
            outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughKeyToDoTenTimesGacha));
        }
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
}