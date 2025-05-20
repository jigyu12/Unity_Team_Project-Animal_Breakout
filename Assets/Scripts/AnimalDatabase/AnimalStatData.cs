using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalStatData", menuName = "Game/AnimalStatData")]
public class AnimalStatData : ScriptableObject
{
    public int AnimalID;
    public string StringID;
    public int Grade;
    public int AttackPower;
    public float StartSpeed;
    public float MaxSpeed;
    public float Jump;
    public string Prefab;

    public PassiveType passive;
    public SkillData SkillData;

    public Sprite iconImage;
    public Sprite tokenIconImage;
    public Sprite starIconImage;

#if UNITY_EDITOR
    private string skillDataPath = "Assets/Resources/ScriptableData/Skill/Skill_Attack{0}.asset";
    private string iconPath = "Assets/Resources/Textures/PlayerIcon/{0}.png";
    
    private string bronzeTokenIconPath = "Assets/Resources/Textures/TokenIcon/Icon_Badge03_Bronze.png";
    private string sliverTokenIconPath = "Assets/Resources/Textures/TokenIcon/Icon_Badge03_Silver.png";
    private string goldTokenIconPath = "Assets/Resources/Textures/TokenIcon/Icon_Badge03_Gold.png";
    
    private string star1IconPath = "Assets/Resources/Textures/StarIcon/star1.png";
    private string star2IconPath = "Assets/Resources/Textures/StarIcon/star2.png";
    private string star3IconPath = "Assets/Resources/Textures/StarIcon/star3.png";

    public void SetData(AnimalDataTable.AnimalRawData rawData)
    {
        this.AnimalID = rawData.AnimalID;
        this.StringID = rawData.StringID;
        this.Grade = rawData.Grade;
        this.AttackPower = rawData.AttackPower;
        this.StartSpeed = rawData.StartSpeed;
        this.MaxSpeed = rawData.MaxSpeed;
        this.Jump = rawData.Jump;
        this.passive = (PassiveType)rawData.PassiveType;
        this.Prefab = rawData.Prefab;

        string path = string.Format(skillDataPath, rawData.SkillID);
        SkillData = AssetDatabase.LoadAssetAtPath<SkillData>(string.Format(skillDataPath, rawData.SkillID));

        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(iconPath, rawData.Prefab));
        iconImage = sprite;
        
        Sprite tokenSprite;
        Sprite starSprite;
        if (Grade == 1)
        {
            tokenSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(bronzeTokenIconPath));
            starSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(star1IconPath));
        }
        else if (Grade == 2)
        {
            tokenSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(sliverTokenIconPath));
            starSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(star2IconPath));
        }
        else 
        {
            tokenSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(goldTokenIconPath));
            starSprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(star3IconPath));
        }
        this.tokenIconImage = tokenSprite;
        this.starIconImage = starSprite;
    }
#endif
}
