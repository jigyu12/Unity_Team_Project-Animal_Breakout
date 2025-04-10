using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapObjectsDataTable : DataTable
{
    private static readonly Dictionary<int, List<MapObjectData>> table = new();
    
    public static event Action<int> OnMaxMapObjectIdSet;
    public static event Action<int> OnMinMapObjectIdSet;

    int maxId = -1;
    int minId = int.MaxValue;
    
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<MapObjectCSVData>(textAsset.text);
        table.Clear();
        foreach (var csvData in list)
        {
            MapObjectData data = new MapObjectData();
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
                List<MapObjectData> newList = new ();
                newList.Add(data);
                
                table.Add(id, newList);
            }
            else
            {
                table[id].Add(data);
            }
        }
        
        OnMaxMapObjectIdSet?.Invoke(maxId);
        OnMinMapObjectIdSet?.Invoke(minId);
    }

    public List<MapObjectData> Get(int prefabID)
    {
        if (!table.ContainsKey(prefabID))
            return null;
        return table[prefabID];
    }
    
    public IEnumerable<KeyValuePair<int, List<MapObjectData>>> GetTableEntries()
    {
        foreach (var entry in table)
        {
            yield return entry;
        }
    }
}
