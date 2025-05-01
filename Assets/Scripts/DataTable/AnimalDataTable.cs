using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalDataTable : DataTable
{
    [Serializable]
    public class AnimalRawData
    {
        public int AnimalID { get; set; }
        public string StringID { get; set; }
        public int Grade { get; set; }
        public int AttackPower { get; set; }
        public float StartSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float Jump { get; set; }
        public int PassiveType { get; set; }
        public int SkillID { get; set; }
        public string Prefab { get; set; }
    }

    private static readonly Dictionary<int, AnimalRawData> table = new();

    public List<int> Keys
    {
        get => keys;
    }
    private List<int> keys;

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<AnimalRawData>(textAsset.text);
        table.Clear();
        foreach (var data in list)
        {
            if (!table.ContainsKey(data.AnimalID))
            {
                table.Add(data.AnimalID, data);
            }
            else
            {
                Debug.LogError($"Ű �ߺ�: {data.AnimalID}");
            }
        }

        keys = table.Keys.ToList();
    }

    public AnimalRawData Get(int animalID)
    {
        if (!table.ContainsKey(animalID))
            return null;
        return table[animalID];
    }

    public List<int> GetAnimalIDs()
    {
        return table.Keys.ToList();
    }

}
