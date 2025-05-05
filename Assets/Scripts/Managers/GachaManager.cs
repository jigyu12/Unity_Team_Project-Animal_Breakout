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
        
        onGachaDo?.Invoke(doGachaDataList);
        
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
        
        for (int i = 0; i < 10; ++i)
        {
            var randomIndex = Utils.GetIndexRandomChanceHitInCumulativeChanceList(cumulativeChanceList);

            doGachaDataList.Add(gachaDataList[randomIndex]);
        }
        
        onGachaDo?.Invoke(doGachaDataList);
        
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
}