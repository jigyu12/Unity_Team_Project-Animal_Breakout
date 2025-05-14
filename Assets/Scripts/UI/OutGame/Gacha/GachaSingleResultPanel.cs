using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GachaSingleResultPanel : GachaPanelBase
{
    [SerializeField] private Image starImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    
    private int currentGachaDataIndex;

    protected override void Awake()
    {
        base.Awake();
        
        var actionMap = inputActions.FindActionMap("UIActions");
        touchAction = actionMap?.FindAction("TouchGacha2");
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        currentGachaDataIndex = 0;

        if (gachaDataList is not null && animalFirstUnlockInfoList is not null)
        {
            var animalUserData = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID);

            if (animalFirstUnlockInfoList[currentGachaDataIndex])
            {
                SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.StringID));
                SetItemImage(animalUserData.AnimalStatData.iconImage);
                SetStarImage(animalUserData.AnimalStatData.starIconImage);
                
                var passiveEffectData = DataTableManager.passiveEffectDataTable.GetPassiveEffectData((int)animalUserData.AnimalStatData.passive, animalUserData.AnimalStatData.Grade, 1);
                float value = (PassiveType)passiveEffectData.PassiveType is PassiveType.SkillDamage or PassiveType.ResultScoreUp or PassiveType.CoinValue ? passiveEffectData.Value * 100f : passiveEffectData.Value;
                SetItemDescriptionText($"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalAttackPowerStringKey)}     {animalUserData.AnimalStatData.AttackPower}\n{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalEndowmentStringKey)}     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, passiveEffectData.StringID, value)}\n{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSkillStringKey)}     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.SkillData.nameID, animalUserData.AnimalStatData.SkillData.level)}");
            }
            else
            {
                if (gachaDataList[currentGachaDataIndex].TokenType == TokenType.BronzeToken)
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalBronzeDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                else if (gachaDataList[currentGachaDataIndex].TokenType == TokenType.SilverToken)
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalSliverDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                else
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalGoldDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                
                SetItemImage(animalUserData.AnimalStatData.tokenIconImage);
                SetStarImage(animalUserData.AnimalStatData.starIconImage);
                SetItemDescriptionText($"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTokenChangeStringKey)} :          {gachaDataList[currentGachaDataIndex].TokenValue}");
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        currentGachaDataIndex = 0;
    }
    
    protected override void OnTouchPerformed(InputAction.CallbackContext context)
    {
        ++currentGachaDataIndex;
        
        if (currentGachaDataIndex == gachaDataList.Count)
        {
            gachaPanelController?.ShowNextGachaPanel();
        }
        else
        {
            var animalUserData = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID);

            if (animalFirstUnlockInfoList[currentGachaDataIndex])
            {
                SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.StringID));
                SetItemImage(animalUserData.AnimalStatData.iconImage);
                SetStarImage(animalUserData.AnimalStatData.starIconImage);
                
                var passiveEffectData = DataTableManager.passiveEffectDataTable.GetPassiveEffectData((int)animalUserData.AnimalStatData.passive, animalUserData.AnimalStatData.Grade, 1);
                float value = (PassiveType)passiveEffectData.PassiveType is PassiveType.SkillDamage or PassiveType.ResultScoreUp or PassiveType.CoinValue ? passiveEffectData.Value * 100f : passiveEffectData.Value;
                SetItemDescriptionText($"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalAttackPowerStringKey)}     {animalUserData.AnimalStatData.AttackPower}\n{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalEndowmentStringKey)}     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, passiveEffectData.StringID, value)}\n{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalSkillStringKey)}     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.SkillData.nameID, animalUserData.AnimalStatData.SkillData.level)}");
            }
            else
            {
                if (gachaDataList[currentGachaDataIndex].TokenType == TokenType.BronzeToken)
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalBronzeDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                else if (gachaDataList[currentGachaDataIndex].TokenType == TokenType.SilverToken)
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalSliverDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                else
                {
                    SetItemNameText(LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                        Utils.AnimalGoldDuplicateStringKey,
                        LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName,
                            animalUserData.AnimalStatData.StringID)));
                }
                
                SetItemImage(animalUserData.AnimalStatData.tokenIconImage);
                SetStarImage(animalUserData.AnimalStatData.starIconImage);
                SetItemDescriptionText($"{LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, Utils.AnimalTokenChangeStringKey)} :          {gachaDataList[currentGachaDataIndex].TokenValue}");
            }
        }
    }
    
    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }
    
    public void SetItemImage(Sprite itemImage)
    {
        this.itemImage.sprite = itemImage;
    }
    
    public void SetItemNameText(string itemNameText)
    {
        this.itemNameText.text = itemNameText;
    }
    
    public void SetItemDescriptionText(string itemDescriptionText)
    {
        this.itemDescriptionText.text = itemDescriptionText;
    }
}