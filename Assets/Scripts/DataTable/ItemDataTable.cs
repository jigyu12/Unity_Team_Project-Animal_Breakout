using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDataTable : DataTable
{
    [Serializable]
    public class ItemData
    {
        public int ItemID { get; set; }
        public int Type { get; set; }
        public int Score { get; set; }
        public int Probability { get; set; }
        //  public string Prefab { get; set; }
    }

    private static readonly Dictionary<int, ItemData> table = new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ItemData>(textAsset.text);
        table.Clear();
        foreach (var data in list)
        {
            if (!table.ContainsKey(data.ItemID))
            {
                table.Add(data.ItemID, data);
            }
            else
            {
                Debug.LogError($"Ű �ߺ�: {data.ItemID}");
            }
        }
    }

    public ItemData Get(int itemID)
    {
        if (!table.ContainsKey(itemID))
            return null;
        return table[itemID];
    }

    public List<int> GetItemIDs()
    {
        return table.Keys.ToList();
    }
}
