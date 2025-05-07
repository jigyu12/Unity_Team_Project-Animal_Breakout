public abstract class SwitchableCanvas : DefaultCanvas
{
    public abstract SwitchableCanvasType switchableCanvasType { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        OutGameUIManager.onSwitchActiveSwitchableCanvas += SwitchActiveSwitchCanvasHandler;
        OutGameUIManager.onSwitchVisualizeSwitchableCanvas += SwitchVisualizeSwitchableCanvasHandler;
        MenuPanel.onMenuBottomButtonClicked += SwitchActiveSwitchCanvasHandler;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        OutGameUIManager.onSwitchActiveSwitchableCanvas -= SwitchActiveSwitchCanvasHandler;
        OutGameUIManager.onSwitchVisualizeSwitchableCanvas -= SwitchVisualizeSwitchableCanvasHandler;
        MenuPanel.onMenuBottomButtonClicked -= SwitchActiveSwitchCanvasHandler;
    }

    protected void SwitchActiveSwitchCanvasHandler(SwitchableCanvasType showCanvasType)
    {
        if(switchableCanvasType == showCanvasType)
        {
            if (outGameManager is not null)
            {
                outGameManager.OutGameUIManager.CurrentSwitchableCanvasType = switchableCanvasType;

                outGameManager.OutGameUIManager.SetCurrentDefaultCanvasTypeInSwitchableCanvasType(showCanvasType);
            }
            
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void SwitchVisualizeSwitchableCanvasHandler(SwitchableCanvasType showCanvasType, bool isVisibleOtherCanvas, bool isVisibleShowCanvasType = true)
    {
        
        if (switchableCanvasType == showCanvasType)
        {
            if (outGameManager is not null)
            {
                outGameManager.OutGameUIManager.CurrentSwitchableCanvasType = switchableCanvasType;
                
                outGameManager.OutGameUIManager.SetCurrentDefaultCanvasTypeInSwitchableCanvasType(showCanvasType);
            }
            
            VisualizeCanvas(isVisibleShowCanvasType);
        }
        else
        {
            VisualizeCanvas(isVisibleOtherCanvas);
        }
    }
}