using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceStatus : MonoBehaviour
{
    public int level = 1;
    private int maxLevel = 20;

    public float ExperienceRatio
    {
        get => Mathf.Clamp01(ExperienceValue / experienceToNextLevel);
    }

    public int ExperienceValue
    {
        get;
        private set;
    } = 0;


    //임시
    private int experienceToNextLevel = 100;

    public Action<int> onLevelChange;
    public Action<int> onAddValue;

    public void AddValue(int value)
    {
        if (level >= maxLevel)
        {
            return;
        }

        ExperienceValue += value;

        onAddValue?.Invoke(value);

        if (ExperienceValue >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level = Mathf.Clamp(level + 1, 1, maxLevel);
        onLevelChange?.Invoke(level);
    }

}
