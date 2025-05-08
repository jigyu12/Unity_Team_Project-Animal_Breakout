using System;
using UnityEngine;

public class PlayerLevelSystem :ISaveLoad
{
    public DataSourceType SaveDataSouceType
    {
        get => DataSourceType.Local;
    }

    public PlayerLevelExperienceData CurrentLevelData
    {
        get;
        private set;
    }

    public int CurrentLevel
    {
        get;
        private set;
    }
    private int maxLevel = 30;
    public bool IsMaxLevel
    {
        get => CurrentLevel >= maxLevel;
    }

    public float ExperienceRatio
    {
        get => Mathf.Clamp01(ExperienceValue / ExperienceToNextLevel);
    }

    public int ExperienceValue
    {
        get;
        private set;
    }
    
    public int ExperienceToNextLevel
    {
        get => CurrentLevelData.Exp;
    }


    static public Action<int, int> onLevelChange; //level, maxexp
    static public Action<int, int> onExperienceValueChanged; //add, sum

    public PlayerLevelSystem()
    {
        //Load(SaveLoadSystem.Instance.CurrentSaveData.playerLevelSystemSave);
        SaveLoadSystem.Instance.RegisterOnSaveAction(this);
    }

    //추후에 로드가 생기면 사용할 함수
    public void SetInitialValue(int level, int exp)
    {
        CurrentLevel = level;
        ExperienceValue = exp;

        CurrentLevelData = DataTableManager.playerLevelDataTalble.GetLevelData(CurrentLevel);
        //onExperienceValueChanged?.Invoke(value, ExperienceValue);
        //onLevelChange?.Invoke(CurrentLevel, ExperienceToNextLevel);
    }

    public void AddExperienceValue(int value)
    {
        if (IsMaxLevel)
        {
            return;
        }

        ExperienceValue += value;

        if (ExperienceValue >= ExperienceToNextLevel)
        {
            ExperienceValue -= ExperienceToNextLevel;
            LevelUp();
        }

        onExperienceValueChanged?.Invoke(value, ExperienceValue);
    }

    private void LevelUp()
    {
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 1, maxLevel);
        CurrentLevelData = DataTableManager.playerLevelDataTalble.GetLevelData(CurrentLevel);

        //골드 보상 지급
        GameDataManager.Instance.GoldAnimalTokenKeySystem.AddGold(CurrentLevelData.CoinReward);

        //스태미나 보상 지급
        GameDataManager.Instance.StaminaSystem.AddStamina(CurrentLevelData.LifeReward);

        //티켓 보상 미적용

        onLevelChange?.Invoke(CurrentLevel, ExperienceToNextLevel);
    }

    public void Save()
    {
        var saveData = SaveLoadSystem.Instance.CurrentSaveData.playerLevelSystemSave = new();
        saveData.currentLevel = CurrentLevel;
        saveData.experienceValue = ExperienceValue;
    }

    public void Load()
    {
        SetInitialValue(1, 0);
    }

    public void Load(PlayerLevelSystemSave saveData)
    {
        if (saveData == null)
        {
            Load();
            return;
        }

        SetInitialValue(saveData.currentLevel, saveData.experienceValue);
    }
}
