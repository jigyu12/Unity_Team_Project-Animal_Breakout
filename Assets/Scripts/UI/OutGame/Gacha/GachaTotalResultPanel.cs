using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GachaTotalResultPanel : GachaPanelBase
{
    [SerializeField] private List<GameObject> gachaResultSlotPanelList;

    private readonly List<GameObject> gachaResultSlotList = new();

    protected override void Awake()
    {
        base.Awake();
        
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchGacha3");
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        AddAllCurrentGachaResultSlotPanel();
    }

    protected override void OnTouchPerformed(InputAction.CallbackContext context)
    {
        RemoveAllCurrentGachaResultSlotPanel();
        
        base.OnTouchPerformed(context);
    }

    private void AddAllCurrentGachaResultSlotPanel()
    {
        if (gachaDataList is null)
        {
            return;
        }
        
        if (gachaDataList.Count == 0 || gachaDataList.Count > gachaResultSlotPanelList.Count)
        {
            Debug.Assert(false, "InValid Gacha Data List");
        }
        
        gachaResultSlotList.Clear();

        if (outGameUIManager is null)
        {
            GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
            outGameUIManager = outGameManager.OutGameUIManager;
        }
        
        for(int i = 0; i < gachaDataList.Count; ++i)
        {
            var gachaResultSlot = outGameUIManager.GetGachaResultSlot();
            
            gachaResultSlot.transform.SetParent(gachaResultSlotPanelList[i].transform);
            gachaResultSlot.transform.localScale = Vector3.one;
            gachaResultSlot.transform.localPosition = Vector3.zero;
            
            gachaResultSlotList.Add(gachaResultSlot);
            gachaResultSlot.TryGetComponent(out GachaResultSlotPanel gachaResultSlotPanel);
            
            if (animalFirstUnlockInfoList[i])
            {
                gachaResultSlotPanel.SetItemName(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, 
                    GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                        gachaDataList[i].AnimalID).AnimalStatData
                    .StringID));
                
                
                gachaResultSlotPanel.SetItemImage(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                    gachaDataList[i].AnimalID).AnimalStatData
                    .iconImage);
                
                gachaResultSlotPanel.SetStarImage(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                        gachaDataList[i].AnimalID).AnimalStatData
                    .starIconImage);
            }
            else
            {
                if (gachaDataList[i].TokenType == TokenType.BronzeToken)
                {
                    gachaResultSlotPanel.SetItemName(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalBronzeDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[i].AnimalID).AnimalStatData.StringID)));
                }
                else if (gachaDataList[i].TokenType == TokenType.SilverToken)
                {
                    gachaResultSlotPanel.SetItemName(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalSliverDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[i].AnimalID).AnimalStatData.StringID)));
                }
                else
                {
                    gachaResultSlotPanel.SetItemName(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalGoldDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[i].AnimalID).AnimalStatData.StringID)));
                }
                
                gachaResultSlotPanel.SetItemImage(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                        gachaDataList[i].AnimalID).AnimalStatData
                    .tokenIconImage);
                
                gachaResultSlotPanel.SetStarImage(GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(
                        gachaDataList[i].AnimalID).AnimalStatData
                    .starIconImage);
            }
            
        }
    }
    
    private void RemoveAllCurrentGachaResultSlotPanel()
    {
        for(int i = 0; i < gachaResultSlotList.Count; ++i)
        {
            outGameUIManager.ReleaseGachaResultSlot(gachaResultSlotList[i]);
        }
        
        gachaResultSlotList.Clear();
    }
}