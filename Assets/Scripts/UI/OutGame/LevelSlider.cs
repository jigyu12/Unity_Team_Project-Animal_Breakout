using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private Slider levelSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expPercentText;

    private readonly StringBuilder stringBuilder = new(4);

    private int level;
    private int maxExp;
    private int remainExpValue;
    
    private void Awake()
    {
        PlayerLevelSystem.onExperienceValueChanged += OnExperienceValueChangedHandler;
        PlayerLevelSystem.onLevelChange += OnLevelChangeHandler;
    }
    
    
    private void Start()
    {
        OnExperienceValueChangedHandler(0, GameDataManager.Instance.PlayerLevelSystem.ExperienceValue);
        OnLevelChangeHandler(GameDataManager.Instance.PlayerLevelSystem.CurrentLevel, GameDataManager.Instance.PlayerLevelSystem.ExperienceToNextLevel);
    }


    private void OnDestroy()
    {
        PlayerLevelSystem.onExperienceValueChanged -= OnExperienceValueChangedHandler;
        PlayerLevelSystem.onLevelChange -= OnLevelChangeHandler;
    }
    
    private void OnExperienceValueChangedHandler(int expToAddValue, int remainExpValue)
    {
        this.remainExpValue = remainExpValue;
        
        SetLevelSlider();
    }

    private void OnLevelChangeHandler(int level, int maxExp)
    {
        this.level = level;
        this.maxExp = maxExp;
        
        SetLevelSlider();
    }
    
    private void SetLevelSlider()
    {
        stringBuilder.Clear();
        stringBuilder.Append(level);
        levelText.SetText(stringBuilder);

        float percentValue = remainExpValue / (float)maxExp;
        stringBuilder.Clear();
        stringBuilder.Append(Mathf.FloorToInt(percentValue * 100));
        stringBuilder.Append('%');
        expPercentText.SetText(stringBuilder);

        levelSlider.value = percentValue;
    }
}