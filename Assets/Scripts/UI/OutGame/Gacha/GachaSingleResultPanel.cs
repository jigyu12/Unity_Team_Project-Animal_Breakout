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
            if (animalFirstUnlockInfoList[currentGachaDataIndex])
            {
                var animalUserData = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID);
                
                SetItemNameText(animalUserData.AnimalStatData.StringID);
                SetItemImage(animalUserData.AnimalStatData.iconImage);
                SetItemDescriptionText($"공격력     {animalUserData.AnimalStatData.AttackPower}\n보유효과     {animalUserData.AnimalStatData.passive.ToString()}\n스킬     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.SkillData.nameID, animalUserData.AnimalStatData.SkillData.level)}");
            }
            else
            {
                SetItemNameText(gachaDataList[currentGachaDataIndex].TokenType.ToString() + $" By {GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID).AnimalStatData.StringID}");
                SetItemImage(null);
                SetItemDescriptionText($"변환 개수 :          {gachaDataList[currentGachaDataIndex].TokenValue}");
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
            if (animalFirstUnlockInfoList[currentGachaDataIndex])
            {
                var animalUserData = GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID);
                
                SetItemNameText(animalUserData.AnimalStatData.StringID);
                SetItemImage(animalUserData.AnimalStatData.iconImage);
                SetItemDescriptionText($"공격력     {animalUserData.AnimalStatData.AttackPower}\n보유효과     {animalUserData.AnimalStatData.passive.ToString()}\n스킬     {LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, animalUserData.AnimalStatData.SkillData.nameID, animalUserData.AnimalStatData.SkillData.level)}");
            }
            else
            {
                SetItemNameText(gachaDataList[currentGachaDataIndex].TokenType.ToString() + $" By {GameDataManager.Instance.AnimalUserDataList.GetAnimalUserData(gachaDataList[currentGachaDataIndex].AnimalID).AnimalStatData.StringID}");
                SetItemImage(null);
                SetItemDescriptionText($"변환 개수 :          {gachaDataList[currentGachaDataIndex].TokenValue}");
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