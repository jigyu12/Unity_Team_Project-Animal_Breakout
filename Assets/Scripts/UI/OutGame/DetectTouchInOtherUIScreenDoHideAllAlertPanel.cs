public class DetectTouchInOtherUIScreenDoHideAllAlertPanel : DetectTouchInOtherUIScreen
{
    protected override void DoInOtherUIScreenTouch()
    {
        outGameUIManager.HideAlertPanelSpawnPanelRoot();
    }
}