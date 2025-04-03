using System;

[Serializable]
public class RewardItemCSVData
{
    public int PrefabID { get; set; }
    public int Start_Coor1 { get; set; }
    public int Start_Coor2 { get; set; }
    public int End_Coor1 { get; set; }
    public int End_Coor2 { get; set; }
    public int Count { get; set; }
    public int Pattern { get; set; }

    public override string ToString()
    {
        return $"{PrefabID}, {Start_Coor1}, {Start_Coor2}, {End_Coor1}, {End_Coor2}, {Count}, {Pattern}";
    }
}

[Serializable]
public class RewardItemData
{
    public int PrefabID { get; set; }
    public int Start_Coor1 { get; set; }
    public int Start_Coor2 { get; set; }
    public int End_Coor1 { get; set; }
    public int End_Coor2 { get; set; }
    public int Count { get; set; }
    public RewardCoinPatternCSVType Pattern { get; set; }

    public void CSVDataToData(RewardItemCSVData csvData)
    {
        PrefabID = csvData.PrefabID;
        Start_Coor1 = csvData.Start_Coor1;
        Start_Coor2 = csvData.Start_Coor2;
        End_Coor1 = csvData.End_Coor1;
        End_Coor2 = csvData.End_Coor2;
        Count = csvData.Count;
        Pattern = (RewardCoinPatternCSVType)csvData.Pattern;
    }
        
    public override string ToString()
    {
        return $"{PrefabID}, {Start_Coor1}, {Start_Coor2}, {End_Coor1}, {End_Coor2}, {Count}, {Pattern}";
    }
}