using UnityEngine;

public class AnimalUserData : ISaveLoad
{
    public AnimalStatData AnimalStatData
    {
        get;
        private set;
    }

    public bool IsUnlock
    {
        get;
        private set;
    }

    public readonly int maxLevel = 10;
    public int Level
    {
        get;
        private set;
    }

    public int AttackPower
    {
        get;
        private set;
    }

    public bool IsMaxLevel
    {
        get => Level >= maxLevel;
    }

    public AnimalUserData(AnimalStatData animalStatData)
    {
        AnimalStatData = animalStatData;
    }

    public void UnlockAnimal()
    {
        if (!IsUnlock)
        {
            Load(); //기본값 채워넣고
            IsUnlock = true;
        }
        else
        {
            Debug.Assert(false, $"Animal is already unlocked in : {AnimalStatData.AnimalID}");
        }
    }

    public void LevelUp()
    {
        Level++;
        UpdateAttackPower();
    }

    private void UpdateAttackPower()
    {
        AttackPower = AnimalStatData.AttackPower;

        if (Level > 1)
        {
            AttackPower += DataTableManager.enforceAnimalDataTable.Get(AnimalStatData.Grade, Level).AttackPower;
        }
    }

    public void Save()
    {
        var saveData = SaveLoadSystem.Instance.CurrentData.animalUserDataTableSave;
        saveData.animalUserDataTable.Add(AnimalStatData.AnimalID, new AnimalUserDataSave { animalID = AnimalStatData.AnimalID, level = Level, isUnlock = IsUnlock });
    }

    public void Load()
    {
        Level = 1;
        IsUnlock = false;
        UpdateAttackPower();
    }

    public void Load(AnimalUserDataSave saveData)
    {
        if (saveData == null)
        {
            Load();
            return;
        }

        Level = saveData.level;
        IsUnlock = saveData.isUnlock;
        UpdateAttackPower();
    }
}