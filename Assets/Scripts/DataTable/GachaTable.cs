using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GachaTable : DataTable
{
    private static readonly Dictionary<int, GachaData> table = new();
    
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<GachaCSVData>(textAsset.text);
        table.Clear();

        foreach (var csvData in list)
        {
            GachaData data = new();
            data.CSVDataToData(csvData);
            
            if (!table.ContainsKey(data.GachaID))
            {
                table.Add(data.GachaID, data);
            }
            else
            {
                Debug.Assert(false, "Duplicated GachaID");
            }
        }
    }
    
    public GachaData Get(int gachaID)
    {
        if (!table.ContainsKey(gachaID))
            return null;
        
        return table[gachaID];
    }
    
    public IEnumerable<KeyValuePair<int, GachaData>> GetTableEntries()
    {
        foreach (var entry in table)
        {
            yield return entry;
        }
    }
}