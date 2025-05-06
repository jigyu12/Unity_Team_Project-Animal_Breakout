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
    public const int maxStamina = 999;

    public Action<int, int> onStaminaChanged;

    public void AddStamina(int value)
    {
        CurrentStamina += value;
        CurrentStamina = Math.Clamp(CurrentStamina, minStamina, maxStamina);

        //onStaminaChangedInGameDataManager?.Invoke(CurrentStamina, maxStaminaByLevelDictionary[currentLevel]);
        //onStaminaChanged?.Invoke(CurrentStamina, maxStaminaByLevelDictionary[currentLevel]);
        Debug.Log($"Stamina has been increased. Current Stamina : {CurrentStamina}");
    }
}
