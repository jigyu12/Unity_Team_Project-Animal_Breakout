using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expPercentText;

    private LevelInfoData levelInfoData;

    private readonly StringBuilder stringBuilder = new(4);

    private void Awake()
    {
        OutGameUIManager.onLevelExpInitialized += LevelExpInitializedHandler;
        OutGameUIManager.onExpChanged += ExpChangedHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onLevelExpInitialized -= LevelExpInitializedHandler;
        OutGameUIManager.onExpChanged -= ExpChangedHandler;
    }

    private void LevelExpInitializedHandler(LevelInfoData initialData)
    {
        levelInfoData = new(initialData.maxLevel);
        levelInfoData.SaveLevelInfoData(initialData.currentLevel, initialData.nextExp, initialData.currentExp);

        TryLevelUp();
    }

    private void ExpChangedHandler(int exp)
    {
        if (exp < 0)
        {
            Debug.Assert(false, "Exp cannot be negative.");
            
            return;
        }
        
        levelInfoData.SaveLevelInfoData(levelInfoData.currentLevel, levelInfoData.nextExp, levelInfoData.currentExp + exp);

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
        if (levelInfoData.currentExp >= levelInfoData.nextExp)
        {
            while (levelInfoData.currentExp >= levelInfoData.nextExp)
            {
                if (levelInfoData.currentLevel >= levelInfoData.maxLevel)
                {
                    levelInfoData.SaveLevelInfoData(levelInfoData.maxLevel,
                        OutGameUIManager.expToLevelUpDictionary[levelInfoData.maxLevel],
                        OutGameUIManager.expToLevelUpDictionary[levelInfoData.maxLevel] - 1);

                    break;
                }

                int remainingExp = levelInfoData.currentExp - levelInfoData.nextExp;
                int nextLevel = levelInfoData.currentLevel + 1;
                int nextExp = OutGameUIManager.expToLevelUpDictionary[nextLevel];

                levelInfoData.SaveLevelInfoData(nextLevel, nextExp, remainingExp);
                
                // Todo : Give Level up Reward
                Debug.Log("Level up!");
            }

            return true;
        }

        return false;
    }

    private void SetLevelSlider()
    {
        stringBuilder.Clear();
        stringBuilder.Append(levelInfoData.currentLevel);
        levelText.SetText(stringBuilder);

        float percentValue = levelInfoData.currentExp / (float)levelInfoData.nextExp;
        stringBuilder.Clear();
        stringBuilder.Append(Mathf.FloorToInt(percentValue * 100));
        stringBuilder.Append('%');
        expPercentText.SetText(stringBuilder);

        levelSlider.value = percentValue;
    }
}