using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameUIManager : MonoBehaviour
{
    [SerializeField] private ShopCanvas shopCanvas;
    [SerializeField] private LobbyCanvas lobbyCanvas;
    [SerializeField] private AnimalCanvas animalCanvas;
    [SerializeField] private MenuCanvas menuCanvas;
    
    [SerializeField] private List<DefaultCanvas> switchableCanvasList;
    private readonly List<CanvasGroup> switchableCanvasGroupComponentList = new();
    
    [SerializeField] private List<LayoutGroupController> layoutGroupControllerList;

    private void Start()
    {
        GetSwitchableCanvasGroupComponent();
        
        StartCoroutine(DisableAfterFrameAllLayoutGroup(DefaultCanvasType.Lobby));
    }
    
    private void OnEnable()
    {
        MenuPanel.OnBottomButtonClicked += MenuCanvasBottomButtonClickedHandler;
    }

    private void OnDisable()
    {
        MenuPanel.OnBottomButtonClicked -= MenuCanvasBottomButtonClickedHandler;
    }

    private void MenuCanvasBottomButtonClickedHandler(DefaultCanvasType type)
    {
        for(int i = 0; i < switchableCanvasList.Count; ++i)
        {
            switchableCanvasList[i].gameObject.SetActive(i == (int)type);
        }
    }

    public void EnableAllLayoutGroup(DefaultCanvasType showCanvasType)
    {
        SwitchActiveSwitchableCanvas(true);
        
        SwitchActiveLayoutGroupController(true);
        
        MenuCanvasBottomButtonClickedHandler(showCanvasType);
    }
    
    public void DisableAllLayoutGroup(DefaultCanvasType showCanvasType)
    {
        SwitchActiveSwitchableCanvas(true);
        
        SwitchActiveLayoutGroupController(false);
        
        MenuCanvasBottomButtonClickedHandler(showCanvasType);
    }
    
    public IEnumerator DisableAfterFrameAllLayoutGroup(DefaultCanvasType showCanvasType)
    {
        SwitchActiveSwitchableCanvas(true);
        
        SwitchVisualizeCanvas(showCanvasType, false);
        
        yield return null;

        SwitchActiveLayoutGroupController(false);

        SwitchVisualizeCanvas(showCanvasType, true);
        
        MenuCanvasBottomButtonClickedHandler(showCanvasType);
    }

    private void SwitchVisualizeCanvas(DefaultCanvasType showCanvasType, bool isVisibleOtherCanvas)
    {
        for (int i = 0; i < switchableCanvasList.Count; ++i)
        {
            var switchableCanvas = switchableCanvasList[i];
            
            if (switchableCanvas.canvasType != showCanvasType)
            {
                var switchableCanvasGroup = switchableCanvasGroupComponentList[i];

                if (isVisibleOtherCanvas)
                {
                    switchableCanvasGroup.alpha = 1f;
                }
                else
                {
                    switchableCanvasGroup.alpha = 0f;
                }
                switchableCanvasGroup.interactable = isVisibleOtherCanvas;
                switchableCanvasGroup.blocksRaycasts = isVisibleOtherCanvas;
            }
        }
    }

    private void SwitchActiveSwitchableCanvas(bool isActive)
    {
        foreach (var switchableCanvas in switchableCanvasList)
        {
            switchableCanvas.gameObject.SetActive(isActive);
        }
    }

    private void SwitchActiveLayoutGroupController(bool isActive)
    {
        if (isActive)
        {
            foreach (var layoutGroupController in layoutGroupControllerList)
            {
                layoutGroupController.EnableAllLayoutComponents();
            }
        }
        else
        {
            foreach (var layoutGroupController in layoutGroupControllerList)
            {
                layoutGroupController.DisableAllLayoutComponents();
            }
        }
    }

    private void GetSwitchableCanvasGroupComponent()
    {
        foreach (var switchableCanvas in switchableCanvasList)
        {
            switchableCanvas.TryGetComponent(out CanvasGroup canvasGroup);
            switchableCanvasGroupComponentList.Add(canvasGroup);
        }
    }
}