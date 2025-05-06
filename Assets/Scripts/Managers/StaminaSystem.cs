using System;
using UnityEngine;

public class StaminaSystem
{
    public int CurrentStamina
    {
        get;
        private set;
    }

    public const int minStamina = 0;
    public const int maxStaminaCanFilled = 5;
    public const int maxStamina = 999;

    private float timeToGetNextStamina = 480f;

    static public Action<int, int > onStaminaChanged;

    //lastTime을 시간으로 바꾸든가해서~ 코드를 바꿔야 할것이다,
    public void SetInitialValue(int stamina, DateTime lastTime)
    {
        CurrentStamina = stamina;

        //자동으로 회복하는 스태미나
        if(CurrentStamina< maxStaminaCanFilled)
        {
            var passedTime = lastTime - DateTime.Now;
            int passedTimeStamina = Mathf.FloorToInt(passedTime.Seconds / timeToGetNextStamina);
            CurrentStamina = Mathf.Clamp(CurrentStamina += passedTimeStamina, minStamina, maxStaminaCanFilled);
        }
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
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
        if(CurrentStamina<value)
        {
            Debug.Log($"Not enough stamina! -> current stamina : {CurrentStamina}, payment : {value} X");
            return;
        }

        CurrentStamina -= value;
        Debug.Log($"pay stamina success! -> current stamina : {CurrentStamina}");
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
    }

}
