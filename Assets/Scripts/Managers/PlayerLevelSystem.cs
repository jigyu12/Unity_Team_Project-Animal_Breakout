using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevelSystem 
{
    public PlayerLevelSystem()
    {
        SceneManager.sceneLoaded += OnChangeSceneHandler;
    }

    ~PlayerLevelSystem()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
    }

    private void OnChangeSceneHandler(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainTitleScene")
        {
            AddExperienceValue(100);
        }
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

    public static Action<int, int> onLevelChange; //level, maxexp
    public static Action<int, int> onExperienceValueChanged; //add, sum

    //추후에 로드가 생기면 사용할 함수
    public void SetInitialValue(int level, int exp)
    {
        CurrentLevel = level;
        ExperienceValue = exp;

        CurrentLevelData = DataTableManager.playerLevelDataTalble.GetLevelData(CurrentLevel);
        
        onExperienceValueChanged?.Invoke(0, ExperienceValue);
        onLevelChange?.Invoke(CurrentLevel, ExperienceToNextLevel);
    }

    public void AddExperienceValue(int value)
    {
        if (IsMaxLevel)
        {
            return;
        }

        ExperienceValue += value;

        while (ExperienceValue >= ExperienceToNextLevel)
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
}