using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaminaSystem
{
    public StaminaSystem()
    {
        SceneManager.sceneLoaded += OnChangeSceneHandler;
    }

    ~StaminaSystem()
    {
        SceneManager.sceneLoaded -= OnChangeSceneHandler;
    }

    private void OnChangeSceneHandler(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainTitleScene")
        {
            onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
        }
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

    public static Action<int, int> onStaminaChanged;

    public Coroutine coAddStamina = null;

    //lastTime을 시간으로 바꾸든가해서~ 코드를 바꿔야 할것이다,
    //첫 저장때는 Now를 넣으면 될듯
    public void SetInitialValue(int stamina, DateTime lastTime)
    {
        CurrentStamina = stamina;

        //자동으로 회복하는 스태미나
        if (CurrentStamina < maxStaminaCanFilled)
        {
            var passedTime = DateTime.Now - lastTime;
            int passedTimeStamina = Mathf.FloorToInt((float)passedTime.TotalSeconds / timeToGetNextStamina);
            CurrentStamina = Mathf.Clamp(CurrentStamina += passedTimeStamina, minStamina, maxStaminaCanFilled);
            currentTimeToGetNextStamina += (float)passedTime.TotalSeconds % timeToGetNextStamina;
        }

        onStaminaChanged += GameDataManager.Instance.AddStaminaRepeat;
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
        if (CurrentStamina < value)
        {
            Debug.Log($"Not enough stamina! -> current stamina : {CurrentStamina}, payment : {value} X");
            return;
        }

        CurrentStamina -= value;
        Debug.Log($"pay stamina success! -> current stamina : {CurrentStamina}");
        onStaminaChanged?.Invoke(CurrentStamina, maxStaminaCanFilled);
    }

    public IEnumerator CoAddStamina()
    {
        //완전 정확하게 하려면 마지막 스테미나를 먹은 시점을 기준으로 계산해야하는데 일단은 이대로 둠
        while (!IsStaminaFull)
        {
            yield return new WaitForSecondsRealtime(currentTimeToGetNextStamina);
            currentTimeToGetNextStamina = timeToGetNextStamina;
            AddStamina(1);
        }

        coAddStamina = null;
    }
}
