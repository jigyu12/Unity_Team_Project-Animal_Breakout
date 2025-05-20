using System.Collections.Generic;
using UnityEngine;

public class SwitchableShopPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> switchableShopPanelList;

    private void Start()
    {
        Initialize();
        
        ShopUISwitchPanel.onSwitchShopPanel += OnSwitchShopPanelHandler;
    }

    private void OnDestroy()
    {
        ShopUISwitchPanel.onSwitchShopPanel -= OnSwitchShopPanelHandler;
    }

    private void Initialize()
    {
        if (switchableShopPanelList.Count == 0)
        {
            return;
        }

        OnSwitchShopPanelHandler(SwitchableShopPanelType.NormalGacha);
    }

    private void OnSwitchShopPanelHandler(SwitchableShopPanelType switchableShopPanelType)
    {
        for(int i = 0; i < switchableShopPanelList.Count; ++i)
        {
            if ((int)switchableShopPanelType == i)
            {
                switchableShopPanelList[i].SetActive(true);
            }
            else
            {
                switchableShopPanelList[i].SetActive(false);
            }
        }
    }
}