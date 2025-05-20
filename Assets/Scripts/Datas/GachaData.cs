using System;

[Serializable]
public class GachaCSVData
{
    public int GachaID { get; set; }
    public int AnimalID { get; set; }
    public float Probability { get; set; }
    public int TokenType { get; set; }
    public int TokenValue { get; set; }
    
    public override string ToString()
    {
        return $"{GachaID}, {AnimalID}, {Probability}, {TokenType}, {TokenValue}";
    }
}

[Serializable]
public class GachaData
{
    public int GachaID { get; set; }
    public int AnimalID { get; set; }
    public float Probability { get; set; }
    public TokenType TokenType { get; set; }
    public int TokenValue { get; set; }
    
    public void CSVDataToData(GachaCSVData csvData)
    {
        GachaID = csvData.GachaID;
        AnimalID = csvData.AnimalID;
        Probability = csvData.Probability;
        TokenType = (TokenType)csvData.TokenType;
        TokenValue = csvData.TokenValue;
    }
    
    public override string ToString()
    {
        return $"{GachaID}, {AnimalID}, {Probability}, {TokenType}, {TokenValue}";
    }
}