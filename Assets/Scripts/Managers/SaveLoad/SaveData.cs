
using System;
using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public GoldAnimalTokenKeySystemSave goldAnimalTokenKeySystemSave = new();
    public AnimalUserDataListSave animalUserDataTableSave = new();
    public DateTime saveTime = DateTime.Now;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();
        return data;
    }
}