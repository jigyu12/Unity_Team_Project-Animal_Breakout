using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardItemsDataTable : DataTable
{
    private static readonly Dictionary<int, List<RewardItemData>> table = new();
    
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<RewardItemCSVData>(textAsset.text);
        table.Clear();
        foreach (var csvData in list)
        {
            RewardItemData data = new RewardItemData();
            data.CSVDataToData(csvData);
            int id = data.PrefabID;
            
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