using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalDataTable : DataTable
{
    [Serializable]
    public class AnimalData
    {
        public int AnimalID { get; set; }
        public string StringID { get; set; }
        public int Grade { get; set; }
        public int AttackPower { get; set; }
        public int StartSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public int Jump { get; set; }
        //public int PassiveType { get; set; }
    }

    private static readonly Dictionary<int, AnimalData> table = new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<AnimalData>(textAsset.text);
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
    }

    public AnimalData Get(int animalID)
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
