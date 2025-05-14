using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnforceSuccessPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text enforceSuccessText;
    [SerializeField] private TMP_Text animalNameText;
    [SerializeField] private TMP_Text attackPowerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text passiveText;
    
    [SerializeField] private Image starImage;
    [SerializeField] private Image animalImage;
    
    [SerializeField] protected InputActionAsset inputActions;
    private InputAction touchAction;
    
    protected OutGameUIManager outGameUIManager;
    
    private bool isFindOtherComponent = false;

    private void Awake()
    {
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchEnforce");
        
        OutGameUIManager.onEnforceSuccessScreenActive += OnEnforceSuccessScreenActiveHandler;
    }

    private void OnDestroy()
    {
        OutGameUIManager.onEnforceSuccessScreenActive -= OnEnforceSuccessScreenActiveHandler;
    }
    
    protected virtual void OnEnable()
    {
        if (touchAction is not null)
        {
            touchAction.performed += OnTouchPerformed;
            touchAction.Enable();
        }
    }

    protected virtual void OnDisable()
    {
        if (touchAction is not null)
        {
            touchAction.performed -= OnTouchPerformed;
            touchAction.Disable();
        }
    }
    
    protected virtual void OnTouchPerformed(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
        outGameUIManager.HideFullScreenPanel();
    }

    private void OnEnforceSuccessScreenActiveHandler(AnimalUserData animalUserData)
    {
        SetEnforceSuccessPanel(animalUserData);
        
        gameObject.SetActive(true);
    }
    
    public void SetEnforceSuccessPanel(AnimalUserData animalUserData)
    {
        if (!isFindOtherComponent)
        {
            GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
            outGameUIManager = outGameManager.OutGameUIManager;
        }
        
        SetEnforceSuccessText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalUpgradeCompleteStringKey));
        SetAnimalNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.StringID));
        SetAttackPowerText(animalUserData, animalUserData.AttackPower);
        SetLevelText(animalUserData, animalUserData.Level);
        SetPassiveText(DataTableManager.passiveEffectDataTable.GetPassiveEffectData((int)animalUserData.AnimalStatData.passive, animalUserData.AnimalStatData.Grade, animalUserData.Level), animalUserData.Level);
        SetAnimalImage(animalUserData.AnimalStatData.iconImage);
        SetStarImage(animalUserData.AnimalStatData.starIconImage);
    }

    public void SetEnforceSuccessText(string text)
    {
        enforceSuccessText.text = text;
    }
    
    public void SetAnimalNameText(string text)
    {
        animalNameText.text = text;
    }

    public void SetAttackPowerText(AnimalUserData animalUserData, int attackPower)
    {
        if (!animalUserData.IsMaxLevel)
        {
            attackPowerText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalAttackPowerStringKey)}   {attackPower}";
        }
        else
        {
            attackPowerText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalAttackPowerStringKey)}   {attackPower}(MAX)";
        }
    }

    public void SetLevelText(AnimalUserData animalUserData, int level)
    {
        if (!animalUserData.IsMaxLevel)
        {
            levelText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalLevelStringKey)}   Lv.{level}";
        }
        else
        {
            levelText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalLevelStringKey)}   Lv.{level}(MAX)";
        }
    }

    public void SetPassiveText(PassiveEffectData passiveEffectData, int level)
    {
        float value = (PassiveType)passiveEffectData.PassiveType is PassiveType.SkillDamage or PassiveType.ResultScoreUp or PassiveType.CoinValue ? passiveEffectData.Value * 100f : passiveEffectData.Value;

        if (level <= 4)
        {
            passiveText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalEndowmentStringKey)}\n<color=grey>{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, passiveEffectData.StringID, value)}</color>";
        }
        else
        {
            passiveText.text = $"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalEndowmentStringKey)}\n{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, passiveEffectData.StringID, value)}"; 
        }
    }
    
    public void SetAnimalImage(Sprite animalImage)
    {
        this.animalImage.sprite = animalImage;
    }

    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }
}