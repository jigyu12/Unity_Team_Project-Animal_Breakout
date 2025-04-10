using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardItemsDataTable : DataTable
{
    private static readonly Dictionary<int, List<RewardItemData>> table = new();
    
    public static event Action<int> OnMaxRewardItemIdSet;
    public static event Action<int> OnMinRewardItemIdSet;
    
    public override void Load(string filename)
    {
        int maxId = -1;
        int minId = int.MaxValue;
        
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<RewardItemCSVData>(textAsset.text);
        table.Clear();
        foreach (var csvData in list)
        {
            RewardItemData data = new RewardItemData();
            data.CSVDataToData(csvData);
            int id = data.PrefabID;
            
            if (id < minId)
            {
                minId = id;
            }

            if (id > maxId)
            {
                maxId = id;
            }
            
            if (!table.ContainsKey(id))
            {
                List<RewardItemData> newList = new ();
                newList.Add(data);
                
                table.Add(id, newList);
            }
            else
            {
                table[id].Add(data);
            }
        }
        
        OnMaxRewardItemIdSet?.Invoke(maxId);
        OnMinRewardItemIdSet?.Invoke(minId);
    }
    
    public List<RewardItemData> Get(int prefabID)
    {
        if (!table.ContainsKey(prefabID))
            return null;
        return table[prefabID];
    }
    
    public IEnumerable<KeyValuePair<int, List<RewardItemData>>> GetTableEntries()
    {
        foreach (var entry in table)
        {
            yield return entry;
        }
    }
}