using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalUserDataListSave
{
    public List<AnimalUserDataSave> animalUserDataList=new();

    public int currentAnimalID;
}

[Serializable]
public class AnimalUserDataSave
{
    public int animalID;
    public int level;
    public bool isUnlock;
}