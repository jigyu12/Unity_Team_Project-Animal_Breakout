using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapObjectsDataTable : DataTable
{
    private static readonly Dictionary<int, List<MapObjectsData>> table = new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<MapObjectsCSVData>(textAsset.text);
        table.Clear();
        foreach (var csvData in list)
        {
            MapObjectsData data = new MapObjectsData();
            data.CSVDataToData(csvData);
            int id = data.PrefabID;
            
            if (!table.ContainsKey(id))
            {
                List<MapObjectsData> newList = new ();
                newList.Add(data);
                
                table.Add(id, newList);
            }
            else
            {
                table[id].Add(data);
            }
        }
    }

    public List<MapObjectsData> Get(int prefabID)
    {
        if (!table.ContainsKey(prefabID))
            return null;
        return table[prefabID];
    }
    
    public IEnumerable<KeyValuePair<int, List<MapObjectsData>>> GetTableEntries()
    {
        foreach (var entry in table)
        {
            yield return entry;
        }
    }
}
