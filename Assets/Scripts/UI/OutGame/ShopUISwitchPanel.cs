using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUISwitchPanel : MonoBehaviour
{
    public static event Action<SwitchableShopPanelType> onSwitchShopPanel;
    
    [SerializeField] private List<Button> switchShopPanelButtonList;
    private readonly List<SwitchShopPanelButton> switchShopPanelButtonComponentList = new();
    
    private void Start()
    {
        Initialize();

        for (int i = 0; i < switchShopPanelButtonList.Count; ++i)
        {
            switchShopPanelButtonComponentList[i].onSwitchShopPanelButtonClicked += OnSwitchShopPanelButtonClickedHandler;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < switchShopPanelButtonList.Count; ++i)
        {
            switchShopPanelButtonComponentList[i].onSwitchShopPanelButtonClicked -= OnSwitchShopPanelButtonClickedHandler;
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < switchShopPanelButtonList.Count; ++i)
        {
            if (i == 0)
            {
                switchShopPanelButtonList[i].interactable = false;
            }
            else
            {
                switchShopPanelButtonList[i].interactable = true;
            }
            
            switchShopPanelButtonList[i].TryGetComponent(out SwitchShopPanelButton switchShopPanelButton);
            switchShopPanelButtonComponentList.Add(switchShopPanelButton);
        }
    }

    private void OnSwitchShopPanelButtonClickedHandler(SwitchableShopPanelType switchableShopPanelType)
    {
        for (int i = 0; i < switchShopPanelButtonList.Count; ++i)
        {
            if ((int)switchableShopPanelType == i)
            {
                switchShopPanelButtonList[i].interactable = false;
            }
            else
            {
                switchShopPanelButtonList[i].interactable = true;
            }
        }
        
        onSwitchShopPanel?.Invoke(switchableShopPanelType);
    }
}