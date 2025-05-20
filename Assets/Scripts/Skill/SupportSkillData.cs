
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "InGameAssets/SupportSkill Data")]

public class SupportSkillData : SkillData
{
    public SupportSkillTarget skillTarget;
    public float rate;

#if UNITY_EDITOR
    private string iconPath = "Assets/Resources/Textures/SkillIcon/{0}.png";

    public void SetData(SupportSkillRawData rawData)
    {
        skillType = SkillType.Support;
        selectPossible = rawData.SelectPossible;
        nameID = rawData.NameID;
        descriptionID = rawData.DescriptionID;

        skillID = rawData.SupportID;
        nameID = rawData.NameID;
        descriptionID = rawData.DescriptionID;
        level = rawData.Level;
        skillGroup = rawData.SupportGroup;

        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(string.Format(iconPath, rawData.Prefab));
        iconImage = sprite;
        skillTarget = (SupportSkillTarget)rawData.SupportType;
        rate = rawData.Value;
    }
#endif

    public override string GetDescriptionString()
    {
        return LocalizationUtility.GetLZString(LocalizationUtility.defaultStringTableName, descriptionID, (int)(rate*100f));
    }
}