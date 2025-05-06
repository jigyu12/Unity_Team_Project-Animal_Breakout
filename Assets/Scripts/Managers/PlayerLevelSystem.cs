using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PlayerLevelSystem 
{
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
        get => Mathf.Clamp01(ExperienceValue / experienceToNextLevel);
    }

    public int ExperienceValue
    {
        get;
        private set;
    }

    private int experienceToNextLevel = 10;

    public Action<int, int> onLevelChange; //level, maxexp
    public Action<int, int> onExperienceValueChanged; //add, sum

    public void AddValue(int value)
    {
        if (IsMaxLevel)
        {
            return;
        }

        ExperienceValue += value;

        if (ExperienceValue >= experienceToNextLevel)
        {
            ExperienceValue -= experienceToNextLevel;
            LevelUp();
        }
        onExperienceValueChanged?.Invoke(value, ExperienceValue);
    }

    private void LevelUp()
    {
        //CurrentLevel = Mathf.Clamp(level + 1, 1, maxLevel);
        //CurrentLevelData = DataTableManager.playerLevelDataTalble.GetLevelData(CurrentLevel);
        //experienceToNextLevel = CurrentLevelData.Exp;
        //onLevelChange?.Invoke(CurrentLevel, experienceToNextLevel);
    }


}
