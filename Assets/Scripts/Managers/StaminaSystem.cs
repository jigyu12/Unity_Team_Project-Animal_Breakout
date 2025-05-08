using System;
using System.Collections;
using UnityEngine;

public class StaminaSystem : ISaveLoad
{
    public DataSourceType SaveDataSouceType
    {
        get => DataSourceType.Local;
    }

    public int CurrentStamina
    {
        get;
        private set;
    }

    public bool IsStaminaFull
    {
        get => CurrentStamina >= maxStaminaCanFilled;
    }

    public const int minStamina = 0;
    public const int maxStaminaCanFilled = 5;
    public const int maxStamina = 999;

    private float timeToGetNextStamina = 480f;
    private float currentTimeToGetNextStamina = 0f;

    public static Action<int, int> onStaminaChanged; //currentStamina, maxStaminaCanFilled

    private DateTime lastStaminaAddTime = DateTime.MinValue;
    public Coroutine coAddStamina = null;

    public StaminaSystem()
    {
        //Load(SaveLoadSystem.Instance.CurrentSaveData.staminaSystemSave);
        SaveLoadSystem.Instance.RegisterOnSaveAction(this);
    }

    public void SetInitialValue(int stamina, DateTime lastTime)
    {
        CurrentStamina = stamina;
        currentTimeToGetNextStamina = timeToGetNextStamina;
        //지난 저장시간 기준으로 스태미나를 회복
        if (!IsStaminaFull)
        {
            var passedTime = DateTime.UtcNow - lastTime;
            int passedTimeStamina = Mathf.FloorToInt((float)passedTime.TotalSeconds / timeToGetNextStamina);
            CurrentStamina = Mathf.Clamp(CurrentStamina += passedTimeStamina, minStamina, maxStaminaCanFilled);
            if (!IsStaminaFull)
            {
                currentTimeToGetNextStamina += (float)passedTime.TotalSeconds % timeToGetNextStamina;
            }
        }

        onStaminaChanged += AddStaminaRepeat;
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
    }

    public float GetLeftTimeToGetNextStamina()
    {
        if (IsStaminaFull)
        {
            return 0f;
        }
        else
        {
            var passedTime = DateTime.UtcNow-lastStaminaAddTime;
            return currentTimeToGetNextStamina - (float)passedTime.TotalSeconds;
        }
    }

    public void AddStamina(int value)
    {
        CurrentStamina += value;
        CurrentStamina = Math.Clamp(CurrentStamina, minStamina, maxStamina);

        //onStaminaChangedInGameDataManager?.Invoke(CurrentStamina, maxStaminaByLevelDictionary[currentLevel]);
        Debug.Log($"Stamina has been increased. Current Stamina : {CurrentStamina}");
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
    }

    public void PayStamina(int value)
    {
        if (CurrentStamina < value)
        {
            Debug.Log($"Not enough stamina! -> current stamina : {CurrentStamina}, payment : {value} X");
            return;
        }

        if (IsStaminaFull)
        {
            lastStaminaAddTime = DateTime.UtcNow;
        }

        CurrentStamina -= value;
        Debug.Log($"pay stamina success! -> current stamina : {CurrentStamina}");
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
    }

    public IEnumerator CoAddStamina()
    {
        Debug.Log($"충전 시작 {DateTime.UtcNow}");

        while (!IsStaminaFull)
        {
            yield return new WaitForSecondsRealtime(currentTimeToGetNextStamina);
            currentTimeToGetNextStamina = timeToGetNextStamina;
            lastStaminaAddTime = DateTime.UtcNow;
            Debug.Log($"충전 1 {DateTime.UtcNow}");
            AddStamina(1);
        }

        coAddStamina = null;
    }
    public void AddStaminaRepeat(int currentStamina, int maxStaminaCanFilled)
    {
        if (coAddStamina == null && !IsStaminaFull)
        {
            coAddStamina = GameDataManager.Instance.StartAddStaminaCoroutine();
        }
    }

    public void Save()
    {
        var saveData = SaveLoadSystem.Instance.CurrentSaveData.staminaSystemSave = new();
        saveData.currentStamina = CurrentStamina;
        saveData.lastStaminaAddTime = lastStaminaAddTime;
    }

    public void Load()
    {
        SetInitialValue(0, DateTime.MinValue);
    }

    public void Load(StaminaSystemSave saveData)
    {
        if (saveData == null)
        {
            Load();
            return;
        }

        lastStaminaAddTime = saveData.lastStaminaAddTime;
        SetInitialValue(saveData.currentStamina, saveData.lastStaminaAddTime);
    }

}
