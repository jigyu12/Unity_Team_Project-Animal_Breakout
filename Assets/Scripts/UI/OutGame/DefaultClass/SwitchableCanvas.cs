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
            VisualizeCanvas(isVisibleShowCanvasType);
        }
        else
        {
            VisualizeCanvas(isVisibleOtherCanvas);
        }
    }

    protected void VisualizeCanvas(bool isVisible)
    {
        if (isVisible)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
        
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}