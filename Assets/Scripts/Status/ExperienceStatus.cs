using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceStatus : MonoBehaviour, IItemTaker
{
    public int level = 1;
    private int maxLevel = 30;

    public float ExperienceRatio
    {
        get => Mathf.Clamp01(ExperienceValue / experienceToNextLevel);
    }

    public float ExperienceValue
    {
        get;
        private set;
    } = 0;

    public bool IsMaxLevel
    {
        get => level >= maxLevel;
    }

    private float additionalExperienceRate = 0f;

    private int experienceToNextLevel = 10;

    private IReadOnlyList<InGameLevelExperienceDataTable.LevelExperence> experences;

    public Action<int, int> onLevelChange; //level, maxexp
    public Action<int, int> onAddValue; //add, sum
    [SerializeField] private TextMeshProUGUI levelGageBar;

    private void Start()
    {
        InitializeValue();
    }

    public void InitializeValue()
    {
        experences = DataTableManager.inGameLevelExperienceDataTable.LevelExperences;
        experienceToNextLevel = experences[level].NextLvExp;

        onLevelChange(level, experienceToNextLevel);
        onAddValue(0, (int)ExperienceValue);
        levelGageBar?.SetText($"{level}");
    }

    public void AddAdditionalExperienceRateValue(float value)
    {
        additionalExperienceRate += value;
    }

    public void AddValue(int value)
    {
        if (IsMaxLevel)
        {
            return;
        }

        ExperienceValue += value + value * additionalExperienceRate;


        if (ExperienceValue >= experienceToNextLevel)
        {
            ExperienceValue -= experienceToNextLevel;
            LevelUp();
        }
        onAddValue?.Invoke(value, (int)ExperienceValue);
    }

    private void LevelUp()
    {
        level = Mathf.Clamp(level + 1, 1, maxLevel);
        experienceToNextLevel = experences[level].NextLvExp;
        onLevelChange?.Invoke(level, experienceToNextLevel);
        levelGageBar?.SetText($"{level}");
    }

    public void ApplyItem(int value)
    {
        AddValue(value);
    }

    [ContextMenu("ApplyValue100")]
    public void ApplyValue100()
    {
        AddValue(100);
    }

}
