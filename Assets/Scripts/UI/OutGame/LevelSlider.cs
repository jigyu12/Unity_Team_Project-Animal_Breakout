using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expPercentText;

    private LevelUpInfoData levelUpInfoData;

    private readonly StringBuilder stringBuilder = new(4);
    
    public static event Action<int, int, int> onLevelUp;
    public static event Action<int, int, int> onExpChangedInSameLevel;

    private void Awake()
    {
        GameDataManager.onLevelExpInitialized += LevelExpInitializedHandler;
        GameDataManager.onExpChanged += ExpChangedHandler;
    }

    private void OnDestroy()
    {
        GameDataManager.onLevelExpInitialized -= LevelExpInitializedHandler;
        GameDataManager.onExpChanged -= ExpChangedHandler;
    }

    private void LevelExpInitializedHandler(LevelUpInfoData initialData)
    {
        levelUpInfoData = new(initialData.maxLevel);
        levelUpInfoData.SaveLevelUpInfoData(initialData.currentLevel, initialData.nextExp, initialData.currentExp);

        TryLevelUp();
    }

    private void ExpChangedHandler(int exp)
    {
        if (exp < 0)
        {
            Debug.Assert(false, "Exp cannot be negative.");
            
            return;
        }
        
        levelUpInfoData.SaveLevelUpInfoData(levelUpInfoData.currentLevel, levelUpInfoData.nextExp, levelUpInfoData.currentExp + exp);

        TryLevelUp();
    }

    private bool TryLevelUp()
    {
        var isLevelUp = UpdateLevelInfoData();
        
        SetLevelSlider();

        return isLevelUp;
    }

    private bool UpdateLevelInfoData()
    {
        if (levelUpInfoData.currentExp >= levelUpInfoData.nextExp)
        {
            while (levelUpInfoData.currentExp >= levelUpInfoData.nextExp)
            {
                if (levelUpInfoData.currentLevel >= levelUpInfoData.maxLevel)
                {
                    levelUpInfoData.SaveLevelUpInfoData(levelUpInfoData.maxLevel,
                        GameDataManager.expToLevelUpDictionary[levelUpInfoData.maxLevel],
                        GameDataManager.expToLevelUpDictionary[levelUpInfoData.maxLevel] - 1);

                    break;
                }

                int remainingExp = levelUpInfoData.currentExp - levelUpInfoData.nextExp;
                int nextLevel = levelUpInfoData.currentLevel + 1;
                int nextExp = GameDataManager.expToLevelUpDictionary[nextLevel];

                levelUpInfoData.SaveLevelUpInfoData(nextLevel, nextExp, remainingExp);
                
                // Todo : Give Level up Reward
                onLevelUp?.Invoke(nextLevel, nextExp, remainingExp);
                Debug.Log("Level up!");
            }

            return true;
        }

        onExpChangedInSameLevel?.Invoke(levelUpInfoData.currentLevel, levelUpInfoData.nextExp, levelUpInfoData.currentExp);
        return false;
    }

    private void SetLevelSlider()
    {
        stringBuilder.Clear();
        stringBuilder.Append(levelUpInfoData.currentLevel);
        levelText.SetText(stringBuilder);

        float percentValue = levelUpInfoData.currentExp / (float)levelUpInfoData.nextExp;
        stringBuilder.Clear();
        stringBuilder.Append(Mathf.FloorToInt(percentValue * 100));
        stringBuilder.Append('%');
        expPercentText.SetText(stringBuilder);

        levelSlider.value = percentValue;
    }
}