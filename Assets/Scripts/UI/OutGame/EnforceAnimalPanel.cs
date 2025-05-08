using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class EnforceAnimalPanel : MonoBehaviour
{
    //모르겠다 일단 이렇게 받아오겠다 수정해달라
    private EnforceAnimalManager enforceAnimalManager;

    [SerializeField] private Image animalImage;
    [SerializeField] private Image starImage;
    [SerializeField] private Image tokenImage;
    [SerializeField] private Image goldImage;

    [SerializeField] private TMP_Text animalNameText;

    [SerializeField] private TMP_Text originAttackPowerText;
    [SerializeField] private TMP_Text enforcedAttackPowerText;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text skillText;
    [SerializeField] private TMP_Text passiveText;
    [SerializeField] private TMP_Text currentTokenText;
    [SerializeField] private TMP_Text requiredTokenText;
    [SerializeField] private TMP_Text enforceText;
    [SerializeField] private TMP_Text goldCostText;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("OutGameManager").TryGetComponent(out OutGameManager outGameManager);
        enforceAnimalManager = outGameManager.EnforceAnimalManager;
    }

    public void SetTargetAnimalUserData(AnimalUserData animalUserData)
    {
        if (!animalUserData.IsMaxLevel)
        {
            //공격력, 비용 텍스트를 업데이트한다
            enforceAnimalManager.ExpectedEnforcedAnimalUserData(animalUserData.AnimalStatData, animalUserData.Level + 1, out int enforcedAttackPower, out int tokenCost, out int goldCost);
            bool isPossible = enforceAnimalManager.IsEnforceAnimalPossible(animalUserData, out bool hasEnouhTokens, out bool hasEnoughGolds);
            
            SetEnforcedAttackPowerText(enforcedAttackPower);
            SetCurrentTokenText(GameDataManager.Instance.GoldAnimalTokenKeySystem.GetCurrentToken(animalUserData.AnimalStatData.Grade), hasEnouhTokens);
            SetRequiredTokenText(tokenCost);
            SetGoldCostText(goldCost, hasEnoughGolds);
        }

        SetAttackPowerText(animalUserData.AnimalStatData.AttackPower);
        SetAnimalImage(animalUserData.AnimalStatData.iconImage);
    }

    public void SetAnimalImage(Sprite animalImage)
    {
        this.animalImage.sprite = animalImage;
    }

    public void SetStarImage(Sprite starImage)
    {
        this.starImage.sprite = starImage;
    }

    public void SetTokenImage(Sprite tokenImage)
    {
        this.tokenImage.sprite = tokenImage;
    }

    public void SetGoldImage(Sprite goldImage)
    {
        this.goldImage.sprite = goldImage;
    }

    public void SetAnimalNameText(string text)
    {
        animalNameText.text = text;
    }

    public void SetAttackPowerText(int power)
    {
        originAttackPowerText.text = power.ToString();
    }

    public void SetEnforcedAttackPowerText(int power)
    {
        enforcedAttackPowerText.text = power.ToString();
    }

    public void SetLevelText(string text)
    {
        levelText.text = text;
    }

    public void SetSkillText(string text)
    {
        skillText.text = text;
    }

    public void SetPassiveText(string text)
    {
        passiveText.text = text;
    }
    public void SetCurrentTokenText(int cost, bool possible)
    {
        currentTokenText.text = cost.ToString();
        currentTokenText.color = possible? Color.black : Color.red;
    }

    public void SetRequiredTokenText(int cost)
    {
        requiredTokenText.text = cost.ToString();
    }

    public void SetEnforceText(string text)
    {
        enforceText.text = text;
    }

    public void SetGoldCostText(int cost, bool possible)
    {
        goldCostText.text = cost.ToString();
        goldCostText.color = possible ? Color.black : Color.red;
    }
}