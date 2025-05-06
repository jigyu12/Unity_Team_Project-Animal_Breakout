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
        if (GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey >= 1)
        {
            outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.DoSingleGacha));
        }
        else
        {
            outGameUIManager.ShowAlertDoubleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughKeyToDoSingleGacha));
        }
    }
    
    public void SetKeyImage(Sprite keyImage)
    {
        this.keyImage.sprite = keyImage;
    }
}