using UnityEngine;

public abstract class DefaultCanvas : MonoBehaviour
{
    public abstract DefaultCanvasType defaultCanvasType { get; protected set; }
    protected abstract CanvasGroup canvasGroup { get; set; }
    
    [SerializeField] private Canvas staticCanvas;
    [SerializeField] private Canvas dynamicCanvas;
    
    protected virtual void Awake()
    {
        TryGetComponent(out CanvasGroup canvasGroup);
        this.canvasGroup = canvasGroup;
        
        OutGameUIManager.onSwitchActiveDefaultCanvases += SwitchActiveDefaultCanvasHandler;
    }
    
    protected virtual void OnDestroy()
    {
        OutGameUIManager.onSwitchActiveDefaultCanvases -= SwitchActiveDefaultCanvasHandler;
    }
    
    protected void SwitchActiveDefaultCanvasHandler(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}