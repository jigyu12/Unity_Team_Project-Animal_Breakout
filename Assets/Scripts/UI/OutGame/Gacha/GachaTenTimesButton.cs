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
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoTenTimesGacha));
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
}