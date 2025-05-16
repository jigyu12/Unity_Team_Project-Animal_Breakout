using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AlertPanelCancelButtonFuncFactory
{
    private static readonly Dictionary<AlertPanelCancelButtonFuncType, UnityAction>
        alertPanelCancelButtonFuncs = new()
        {
            {
                AlertPanelCancelButtonFuncType.CloseAlertPanel, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.isGameQuitPanelShow = false;
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                }
            },
            {
                AlertPanelCancelButtonFuncType.CloseAlertPanelBySetActive, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.isGameQuitPanelShow = false;
                    outGameManager.OutGameUIManager.HideLastAlertPanel();
                }
            },
            {
                AlertPanelCancelButtonFuncType.CloseEnforceAlertPanelBySetActive, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.isGameQuitPanelShow = false;
                    outGameManager.OutGameUIManager.HideLastAlertPanel();
                    outGameManager.OutGameUIManager.lastEnforceAnimalPanel.transform.GetChild(0).TryGetComponent(
                        out EnforceAnimalPanel enforceAnimalPanel);
                    enforceAnimalPanel.EnforceButton.interactable = true;
                }
            }
        };
    
    public static UnityAction GetAlertPanelCancelButtonFunc(AlertPanelCancelButtonFuncType type)
    {
        if (alertPanelCancelButtonFuncs.TryGetValue(type, out var func))
        {
            return func;
        }

        Debug.Assert(false, $"Cant find AlertPanelCancelButtonFunc in AlertPanelCancelButtonFuncType: {type}");
        
        return null;
    }
}