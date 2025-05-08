using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalUserDataListSave
{
    public Dictionary<int, AnimalUserDataSave> animalUserDataTable = new();

    public int currentAnimalID;
}

[Serializable]
public class AnimalUserDataSave
{
    public int animalID;
    public int level;
    public bool isUnlock;
}