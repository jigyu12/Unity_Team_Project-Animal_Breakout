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

                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);

                    // GameDataManager.Instance.GachaSingleAdsButton.interactable = false; 
                    // outGameManager.OutGameUIManager.lastAlertPanel.transform.GetChild(0).TryGetComponent(out AlertPanel alertPanel);
                    // alertPanel.SetInteractableButton(false);

                    GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount--;
                    GameDataManager.Instance.GachaSingleAdsButton.interactable =
                        GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount > 0;
              
                    //보상형 광고 재생, 광고가 실패할경우에 대한 안전장치가 없다
                            GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount--;

                            GameDataManager.Instance.GachaSingleAdsButton.interactable =
                                GameDataManager.Instance.PlayerAccountData.GachaSingleAdsRemainCount > 0;
                   
                    NativeServiceManager.Instance.AdvertisementSystem.ShowRewardedAdvertisement(null, () =>
                        {
                            outGameManager.GachaManager.GenerateRandomSingleGachaData();
                            onGachaByAds?.Invoke();

                            outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                            outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);

                            //alertPanel.SetInteractableButton(true);
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
                        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(GameDataManager.Instance
                            .GoldAnimalTokenKeySystem.CurrentKey);

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
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(
                            AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughGold));
                    }
                }
            },
            {
                AlertPanelConfirmButtonFuncType.NotEnoughKeyToDoTenTimesGacha, () =>
                {
                    var isSuccessPayGold = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayGold
                    (GameDataManager.keyPrice *
                     (10 - GameDataManager.Instance.GoldAnimalTokenKeySystem.CurrentKey));

                    if (isSuccessPayGold)
                    {
                        GameDataManager.Instance.GoldAnimalTokenKeySystem.PayKey(GameDataManager.Instance
                            .GoldAnimalTokenKeySystem.CurrentKey);

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
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(
                            AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughGold));
                    }
                }
            },
            {
                AlertPanelConfirmButtonFuncType.CheckStaminaPurchase, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    if (StaminaSystem.maxStamina <
                        GameDataManager.Instance.StaminaSystem.CurrentStamina + GameDataManager.Instance.staminaToAdd)
                    {
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(
                            AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType
                                .TooManyStaminaToPurchaseStamina));

                        return;
                    }

                    var isSuccessPayGold = GameDataManager.Instance.GoldAnimalTokenKeySystem.PayGold
                        (GameDataManager.Instance.staminaGoldUseCost);
                    if (isSuccessPayGold)
                    {
                        GameDataManager.Instance.StaminaSystem.AddStamina(GameDataManager.Instance.staminaToAdd);
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                    }
                    else
                    {
                        outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                        outGameManager.OutGameUIManager.ShowAlertSingleButtonPanel(
                            AlertPanelInfoDataFactory.GetAlertPanelInfoData(AlertPanelInfoDataType.NotEnoughGold));
                    }
                }
            },
            {
                AlertPanelConfirmButtonFuncType.EnforceAnimal, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.EnforceAnimalManager.EnforceAnimal(GameDataManager.Instance.targetEnforceAnimalPanel
                        .animalUserData);

                    GameDataManager.Instance.targetEnforceAnimalPanel.SetTargetAnimalUserData(GameDataManager.Instance
                        .targetEnforceAnimalPanel.animalUserData);

                    outGameManager.OutGameUIManager.HideLastAlertPanel();

                    outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.EnforceSuccessScreen);

                    outGameManager.OutGameUIManager.SortUnlockAnimalPanel();
                }
            },
            {
                AlertPanelConfirmButtonFuncType.DoSingleTutorialGacha, () =>
                {
                    GameObject.FindGameObjectWithTag("OutGameManager")
                        .TryGetComponent(out OutGameManager outGameManager);
                    outGameManager.GachaManager.GenerateTutorialGachaData();
                    outGameManager.OutGameUIManager.HideAlertPanelSpawnPanelRoot();
                    outGameManager.OutGameUIManager.ShowFullScreenPanel(FullScreenType.GachaScreen);
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