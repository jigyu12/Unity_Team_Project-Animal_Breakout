using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveEffectData
{
    public int PassiveType { get; set; }
    public int Grade { get; set; }
    public int Level { get; set; }

    public float Value { get; set; }
    public string StringID { get; set; }
}

public class PassiveEffectDataTable : DataTable
{
    public class PassiveEffectGroup
    {
        public int passiveType;
        public int grade;
        public List<PassiveEffectData> passiveEffectDatas = new List<PassiveEffectData>();
    }

    private static readonly Dictionary<(int, int), PassiveEffectGroup> table = new();


    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<PassiveEffectData>(textAsset.text);
        table.Clear();
        foreach (var data in list)
        {
            if (!table.ContainsKey((data.PassiveType, data.Grade)))
            {
                var dataGroup = new PassiveEffectGroup();
                dataGroup.passiveType = data.PassiveType;
                dataGroup.grade = data.Grade;
                dataGroup.passiveEffectDatas.Add(data);
                table.Add((data.PassiveType, data.Grade), dataGroup);
            }
            else
            {
                table[(data.PassiveType, data.Grade)].passiveEffectDatas.Add(data);
            }
        }
    }

    public PassiveEffectGroup Get(int passiveType, int grade)
    {
        if (!table.ContainsKey((passiveType, grade)))
            return null;
        return table[(passiveType, grade)];
    }
    
    public PassiveEffectData GetPassiveEffectData(int passiveType, int grade, int level)
    {
        if (level <= 9)
        {
            return Get(passiveType, grade).passiveEffectDatas[0];
        }
        else
        {
            return Get(passiveType, grade).passiveEffectDatas[1];
        }
    }
}