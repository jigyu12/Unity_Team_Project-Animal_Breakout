using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchShopPanelButton : MonoBehaviour
{
    [SerializeField] private Button switchShopPanelButton;
    [SerializeField] private SwitchableShopPanelType switchableShopPanelType;
 
    public Action<SwitchableShopPanelType> onSwitchShopPanelButtonClicked;

    private void Start()
    {
        switchShopPanelButton.onClick.RemoveAllListeners();
        switchShopPanelButton.onClick.AddListener(() => onSwitchShopPanelButtonClicked?.Invoke(switchableShopPanelType));
    }
}