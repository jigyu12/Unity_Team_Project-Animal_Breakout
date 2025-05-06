using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnforceAnimalData
{
    public int Level { get; set; }
    public int Grade { get; set; }
    public int AttackPower { get; set; }
    public int TokenValue { get; set; }
    public int Cost { get; set; }
}

public class EnforceAnimalDataTable : DataTable
{

    //grade lv
    private static readonly Dictionary<(int, int), EnforceAnimalData> table = new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EnforceAnimalData>(textAsset.text);
        table.Clear();
        foreach (var data in list)
        {
            if (!table.ContainsKey((data.Grade, data.Level)))
            {
                table.Add((data.Grade, data.Level), data);
            }
            else
            {
                Debug.LogError($"grade, lv key already exist : {(data.Grade, data.Level)}");
            }
        }
    }

    public EnforceAnimalData Get(int grade, int level)
    {
        if (!table.ContainsKey((grade, level)))
            return null;
        return table[(grade, level)];
    }

}