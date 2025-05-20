using UnityEngine;

public abstract class DefaultCanvas : MonoBehaviour
{
    public abstract DefaultCanvasType defaultCanvasType { get; protected set; }
    protected abstract CanvasGroup canvasGroup { get; set; }
    
    [SerializeField] private Canvas staticCanvas;
    [SerializeField] private Canvas dynamicCanvas;
    
    protected OutGameManager outGameManager;
    
    protected virtual void Awake()
    {
        TryGetComponent(out CanvasGroup canvasGroup);
        this.canvasGroup = canvasGroup;

        if (defaultCanvasType == DefaultCanvasType.FullScreen)
        {
            VisualizeCanvas(true);
        }
        
        OutGameUIManager.onSwitchActiveAllDefaultCanvas += SwitchActiveAllDefaultCanvasHandler;
        OutGameUIManager.onSwitchActiveDefaultCanvas += OnSwitchActiveDefaultCanvasHandler;
    }
    
    protected virtual void OnDestroy()
    {
        OutGameUIManager.onSwitchActiveAllDefaultCanvas -= SwitchActiveAllDefaultCanvasHandler;
        OutGameUIManager.onSwitchActiveDefaultCanvas -= OnSwitchActiveDefaultCanvasHandler;
    }

    protected virtual void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out outGameManager);
    }
    
    protected void SwitchActiveAllDefaultCanvasHandler(bool isActive)
    {
        gameObject.SetActive(isActive);
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
    
    protected void OnSwitchActiveDefaultCanvasHandler(DefaultCanvasType type, bool isActive, bool inActiveOtherCanvas)
    {
        if (type == defaultCanvasType)
        {
            if (isActive)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (inActiveOtherCanvas)
            {
                gameObject.SetActive(false);
            }
        }
    }
}