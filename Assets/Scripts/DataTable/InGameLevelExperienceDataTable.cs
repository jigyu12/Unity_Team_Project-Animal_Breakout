using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameLevelExperienceDataTable : DataTable
{
    [Serializable]
    public class LevelExperence
    {
        public int CharacterLv { get; set; }
        public int NextLvExp { get; set; }
        public int TotalExpNeeded { get; set; }
       
    }

    private static readonly List<LevelExperence> levelList = new();

    public IReadOnlyList<LevelExperence> LevelExperences
    {
        get => levelList;
    }

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<LevelExperence>(textAsset.text);

        levelList.Clear();

        levelList.Add(null);
        foreach (var item in list)
        {
            levelList.Add(item);
        }
        
    }

    public LevelExperence Get(int level)
    {
        if (level>levelList.Count)
            return null;
        return levelList[level];
    }
}
