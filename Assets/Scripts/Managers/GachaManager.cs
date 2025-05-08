using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour, IManager
{
    private OutGameManager outGameManager;
    
    private GachaTable gachaTable;
    private readonly List<GachaData> gachaDataList = new();
    private List<float> cumulativeChanceList;

    private readonly List<GachaData> doGachaDataList = new();
    public static event Action<List<GachaData>> onGachaDo;
    public static event Action<int> onAnimalUnlocked;
    public static event Action<TokenType, int> onTokenAdded;
    
    private readonly List<bool> animalFirstUnlockInfoList = new();
    public static event Action<List<bool>> onAnimalFirstUnlockedListSet;
    
    public static event Action onAnimalUnlockedFinished;

    public void GenerateRandomSingleGachaData()
    {
        StartCoroutine(GenerateRandomSingleGachaDataCoroutine());
    }

    private IEnumerator GenerateRandomSingleGachaDataCoroutine()
    {
        yield return null;
        
        var randomIndex = Utils.GetIndexRandomChanceHitInCumulativeChanceList(cumulativeChanceList);
        
        doGachaDataList.Clear();
        doGachaDataList.Add(gachaDataList[randomIndex]);
        
        animalFirstUnlockInfoList.Clear();
        
        onGachaDo?.Invoke(doGachaDataList);

        SetAnimalUserDataByGachaResult();
        
        onAnimalFirstUnlockedListSet?.Invoke(animalFirstUnlockInfoList);
        
        Debug.Log("Generate Single Gacha Data");
    }
    
    public void GenerateRandomTenTimeGachaData()
    {
        StartCoroutine(GenerateRandomTenTimeGachaDataCoroutine());
    }
    
    private IEnumerator GenerateRandomTenTimeGachaDataCoroutine()
    {
        yield return null;
        
        doGachaDataList.Clear();
        animalFirstUnlockInfoList.Clear();
        
        for (int i = 0; i < 10; ++i)
        {
            var randomIndex = Utils.GetIndexRandomChanceHitInCumulativeChanceList(cumulativeChanceList);

            doGachaDataList.Add(gachaDataList[randomIndex]);
        }
        
        onGachaDo?.Invoke(doGachaDataList);

        SetAnimalUserDataByGachaResult();
        
        onAnimalFirstUnlockedListSet?.Invoke(animalFirstUnlockInfoList);
        
        Debug.Log("Generate Ten Times Gacha Data");
    }
    
    public void Initialize()
    {
        gachaTable = DataTableManager.gachaTable;

        List<float> gachaTableChanceList = new();
        foreach (var kvp in gachaTable.GetTableEntries())
        {
            gachaDataList.Add(kvp.Value);
            gachaTableChanceList.Add(kvp.Value.Probability * 0.01f);
        }
        cumulativeChanceList = Utils.ToCumulativeChanceList(gachaTableChanceList);
    }

    public void Clear()
    {
        
    }
    
    public void SetOutGameManager(OutGameManager outGameManager)
    {
        this.outGameManager = outGameManager;
    }

    private void SetAnimalUserDataByGachaResult()
    {
        for (int i = 0; i < doGachaDataList.Count; ++i)
        {
            var acquiredAnimalId = doGachaDataList[i].AnimalID;
            var animalUserList = GameDataManager.Instance.AnimalUserDataList;
            var animalUserData = animalUserList.GetAnimalUserData(acquiredAnimalId);

            if (animalUserData is null)
            {
                Debug.Assert(false, "Invalid animal Id in Gacha.");
            }
            
            if (!animalUserData.IsUnlock)
            {
                animalUserList.UnlockAnimal(acquiredAnimalId);
                onAnimalUnlocked?.Invoke(acquiredAnimalId);
                
                animalFirstUnlockInfoList.Add(true);
            }
            else
            {
                onTokenAdded?.Invoke(doGachaDataList[i].TokenType, doGachaDataList[i].TokenValue);
                
                animalFirstUnlockInfoList.Add(false);
                
                Debug.Log($"Give Token By AnimalId: {acquiredAnimalId}, name : {animalUserList.GetAnimalUserData(acquiredAnimalId).AnimalStatData.StringID}");
            }
        }
        
        onAnimalUnlockedFinished?.Invoke();
    }
}