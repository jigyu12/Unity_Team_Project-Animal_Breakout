using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
    public SkillData SkillData;

#if UNITY_EDITOR
    private string skillDataPath = "Assets/Resources/ScriptableData/Skill/Skill_Attack{0}.asset";
    public void SetData(AnimalDataTable.AnimalRawData rawData)
    {
        this.AnimalID = rawData.AnimalID;
        this.StringID = rawData.StringID;
        this.Grade = rawData.Grade;
        this.AttackPower = rawData.AttackPower;
        this.StartSpeed = rawData.StartSpeed;
        this.MaxSpeed = rawData.MaxSpeed;
        this.Jump = rawData.Jump;

        string path = string.Format(skillDataPath, rawData.SkillID);
        SkillData = AssetDatabase.LoadAssetAtPath<SkillData>(string.Format(skillDataPath, rawData.SkillID));
    }
#endif
}
