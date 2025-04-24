using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdditionalStatusEffectData
{
    public int EffectID { get; set; }
    public int EffectType { get; set; }
    public int Attribute { get; set; }
    public float Damage { get; set; }

    public int AttackCount { get; set; }
}

public class AdditionalStatusEffectDataTable : DataTable
{

    private static readonly Dictionary<int, AdditionalStatusEffectData> table = new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<AdditionalStatusEffectData>(textAsset.text);
        table.Clear();
        foreach (var data in list)
        {
            if (!table.ContainsKey(data.EffectID))
            {
                table.Add(data.EffectID, data);
            }
            else
            {
                Debug.LogError($"Ű �ߺ�: {data.EffectID}");
            }
        }
    }

    public AdditionalStatusEffectData Get(int itemID)
    {
        if (!table.ContainsKey(itemID))
            return null;
        return table[itemID];
    }
}
