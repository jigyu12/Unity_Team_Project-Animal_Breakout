using System;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Events;

public static class AlertPanelConfirmButtonFuncFactory
{
    public static event Action onGachaByAds;

    private static readonly Dictionary<AlertPanelConfirmButtonFuncType, UnityAction>
        alertPanelConfirmButtonFuncs = new()
        {
            {
                AlertPanelConfirmButtonFuncType.CloseAlertPanel, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                }
            },
            { AlertPanelConfirmButtonFuncType.QuitGame, Application.Quit },
            {
                AlertPanelConfirmButtonFuncType.DoSingleGacha, () =>
                {
                    var isSuccessPayKey = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(1);
                    if (!isSuccessPayKey)
                    {
                        Debug.Assert(false, "Not Enough Key");

                        return;
                    }

                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateRandomSingleGachaData();
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                    outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
                }
            },
            {
                AlertPanelConfirmButtonFuncType.DoSingleGachaByAds, () =>
                {

                    Debug.Log("Gacha By Ads");
                    //보상형 광고 재생
                    NativeServiceManager.Instance.AdvertisementSystem.ShowRewardedAdvertisement(null, ()=>
        {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateRandomSingleGachaData();
                    onGachaByAds?.Invoke();

                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                    outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
        }
        , Time.timeScale);


                }
            },
            {
                AlertPanelConfirmButtonFuncType.DoTenTimesGacha, () =>
                {
                    var isSuccessPayKey = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(10);
                    if (!isSuccessPayKey)
                    {
                        Debug.Assert(false, "Not Enough Key");

                        return;
                    }

                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateRandomTenTimeGachaData();
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                    outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
                }
            },
            {
                AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoSingleGacha, () =>
                {
                    var isSuccessPayGold = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayGold
                        (GameDataManager.keyPrice * (1 - GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey));

                    if (isSuccessPayGold)
                    {
                        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey);

                        GameObject.FindGameObjectWithTag("OutGameManager")
                            .TryGetComponent(out OutGameManager outGameManager);
                        outGameManager.GachaManager.GenerateRandomSingleGachaData();
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("OutGameManager")
                            .TryGetComponent(out OutGameManager outGameManager);
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughGold));
                    }
                }
            },
            {
                AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoTenTimesGacha, () =>
                {
                    var isSuccessPayGold = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayGold
                        (GameDataManager.keyPrice * (10 - GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey));

                    if (isSuccessPayGold)
                    {
                        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey);

                        GameObject.FindGameObjectWithTag("OutGameManager")
                            .TryGetComponent(out OutGameManager outGameManager);
                        outGameManager.GachaManager.GenerateRandomTenTimeGachaData();
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("OutGameManager")
                            .TryGetComponent(out OutGameManager outGameManager);
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughGold));
                    }
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