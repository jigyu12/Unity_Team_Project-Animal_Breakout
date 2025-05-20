using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchShopPanelButton : MonoBehaviour
{
    [SerializeField] private Button switchShopPanelButton;
    [SerializeField] private SwitchableShopPanelType switchableShopPanelType;
 
    public event Action<SwitchableShopPanelType> onSwitchShopPanelButtonClicked;
    
    [SerializeField] private RectTransform rectTransformToRefresh;
    [SerializeField] private RectTransform rectTransform2ToRefresh;

    private void Start()
    {
        switchShopPanelButton.onClick.AddListener(() => onSwitchShopPanelButtonClicked?.Invoke(switchableShopPanelType));

        if (switchableShopPanelType == SwitchableShopPanelType.Stamina)
        {
            if (rectTransformToRefresh is null || rectTransform2ToRefresh is null)
            {
                return;
            }
            
            switchShopPanelButton.onClick.AddListener(() =>
            {
                for (int i = 0; i < 4; ++i)
                {
                    Canvas.ForceUpdateCanvases();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformToRefresh);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform2ToRefresh);
                }
            });
        }
    }
    
    private void OnDestroy()
    {
        switchShopPanelButton.onClick.RemoveAllListeners();
    }
}