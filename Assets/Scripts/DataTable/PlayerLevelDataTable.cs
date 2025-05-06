using System;
using System.Collections.Generic;
using UnityEngine;

    [Serializable]
    public class PlayerLevelExperienceData
    {
        public int LevelID { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int LifeReward { get; set; }
        public int CoinReward { get; set; }
        public int TicketReward { get; set; }
        public int TicketValue { get; set; }
    }
public class PlayerLevelDataTable : DataTable
{
    private static readonly Dictionary<int, PlayerLevelExperienceData> table = new();
    private static readonly List<PlayerLevelExperienceData> list=new();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var _list = LoadCSV<PlayerLevelExperienceData>(textAsset.text);
        table.Clear();

        //순서를 맞추기 위함
        list.Add(null);
        foreach (var data in _list)
        {
            if (!table.ContainsKey(data.LevelID))
            {
                table.Add(data.LevelID, data);
            }
            else
            {
                Debug.LogError($"Ű �ߺ�: {data.LevelID}");
            }
            list.Add(data);
        }
    }

    public PlayerLevelExperienceData Get(int levelID)
    {
        if (!table.ContainsKey(levelID))
            return null;
        return table[levelID];
    }

    public PlayerLevelExperienceData GetLevelData(int level)
    {
        if (level > list.Count)
            return null;
        return list[level];
    }
}
