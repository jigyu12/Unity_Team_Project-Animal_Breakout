using UnityEngine;
using UnityEngine.UI;

public class GachaSingleButton : GachaButton
{
    [SerializeField] private Image keyImage;
    
    protected override void Start()
    {
        base.Start();
        
        SetGachaButtonText("1회", "1");
    }
    
    public override void DoGacha()
    {
        outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGacha));
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
}