using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AlertPanelConfirmButtonFuncFactory
{
    private static readonly Dictionary<AlertPanelConfirmButtonFuncType, UnityAction>
        alertPanelConfirmButtonFuncs = new()
        {
            {
                AlertPanelConfirmButtonFuncType.CloseAlertPanel, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                }
            },
            {
                AlertPanelConfirmButtonFuncType.QuitGame, Application.Quit
            },
            {
                AlertPanelConfirmButtonFuncType.DoSingleGacha, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateRandomSingleGachaData();
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                }
            },
            {
                AlertPanelConfirmButtonFuncType.DoSingleGachaByAds, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateRandomSingleGachaData();
                    
                    Debug.Log("Gacha By Ads");

                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                }
            },
            {
              AlertPanelConfirmButtonFuncType.DoTenTimesGacha, () =>
              {
                  GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
                  outGameManager.GachaManager.GenerateRandomTenTimeGachaData();
                  outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
              }
            },
        };
    
    public static UnityAction GetAlertPanelConfirmButtonFunc(AlertPanelConfirmButtonFuncType type)
    {
        if (alertPanelConfirmButtonFuncs.TryGetValue(type, out var func))
        {
            return func;
        }

        Debug.Assert(false, $"Cant find AlertPanelConfirmButtonFunc in AlertPanelConfirmButtonFuncType: {type}");
        
        return null;
    }
}