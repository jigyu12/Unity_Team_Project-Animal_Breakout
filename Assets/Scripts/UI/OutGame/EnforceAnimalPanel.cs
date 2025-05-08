using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnforceAnimalPanel : MonoBehaviour
{
    [SerializeField] private Image animalImage;
    [SerializeField] private Image starImage;
    [SerializeField] private Image tokenImage;
    [SerializeField] private Image goldImage;

    [SerializeField] private TMP_Text animalNameText;
    [SerializeField] private TMP_Text attackPowerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text skillText;
    [SerializeField] private TMP_Text passiveText;
    [SerializeField] private TMP_Text requiredTokenText;
    [SerializeField] private TMP_Text enforceText;
    [SerializeField] private TMP_Text goldCostText;
    
    

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

    public void SetAttackPowerText(string text)
    {
        attackPowerText.text = text;
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
    
    public void SetRequiredTokenText(string text)
    {
        requiredTokenText.text = text;
    }
    
    public void SetEnforceText(string text)
    {
        enforceText.text = text;
    }
    
    public void SetGoldCostText(string text)
    {
        goldCostText.text = text;
    }
}