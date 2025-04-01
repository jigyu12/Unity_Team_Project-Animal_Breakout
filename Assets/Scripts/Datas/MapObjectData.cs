using System;

[Serializable]
public class MapObjectCSVData
{
    public int PrefabID { get; set; }
    public int Obj_Type { get; set; }
    public int Coor1 { get; set; }
    public int Coor2 { get; set; }

    public override string ToString()
    {
        return $"{PrefabID}, {Obj_Type}, {Coor1}, {Coor2}";
    }
}

[Serializable]
public class MapObjectData
{
    public int PrefabID { get; set; }
    public MapObjectCSVType Obj_Type { get; set; }
    public int Coor1 { get; set; }
    public int Coor2 { get; set; }

    public void CSVDataToData(MapObjectCSVData csvData)
    {
        PrefabID = csvData.PrefabID;
        Obj_Type = (MapObjectCSVType)csvData.Obj_Type;
        Coor1 = csvData.Coor1;
        Coor2 = csvData.Coor2;
    }
        
    public override string ToString()
    {
        return $"{PrefabID}, {Obj_Type}, {Coor1}, {Coor2}";
    }
}