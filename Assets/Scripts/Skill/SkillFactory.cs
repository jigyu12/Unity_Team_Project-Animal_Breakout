using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillFactory
{

    private List<Dictionary<string, SkillDataLevelGroup>> skillDataTable = new();
    private List<List<string>> skillListGroupKeys=new();


    public class SkillDataLevelGroup
    {
        public SkillDataLevelGroup()
        {
            skillDatas = new();
        }
        public void AddToGroup(SkillData skillData)
        {
            skillDatas.Add(skillData);
        }

        public void LevelSort()
        {
            skillDatas.Sort((a, b) => a.level.CompareTo(b.level));
        }

        public SkillData GetSkill(int level)
        {
            return skillDatas[level - 1];
        }

        private List<SkillData> skillDatas;
    }


    public SkillFactory()
    {
        InitializeSkillData();
    }

    private void InitializeSkillData()
    {
        //Resources폴더에 있는 스킬데이터들을 가져온다
        AttackSkillData[] attackSkillDatas = Resources.LoadAll<AttackSkillData>("ScriptableData/Skill/");
        SupportSkillData[] supportSkillDatas = Resources.LoadAll<SupportSkillData>("ScriptableData/Skill/");

        for (int i = 0; i < Enum.GetValues(typeof(SkillType)).Length; i++)
        {
            skillDataTable.Add(new Dictionary<string, SkillDataLevelGroup>());
        }

        foreach (var skillData in attackSkillDatas)
        {
            if (skillDataTable[(int)SkillType.Attack].ContainsKey(skillData.skillGroup))
            {
                skillDataTable[(int)SkillType.Attack][skillData.skillGroup].AddToGroup(skillData);
            }
            else
            {
                var newGroup = new SkillDataLevelGroup();
                newGroup.AddToGroup(skillData);
                skillDataTable[(int)SkillType.Attack].Add(skillData.skillGroup, newGroup);
            }
        }

        foreach (var skillData in supportSkillDatas)
        {
            if (skillDataTable[(int)SkillType.Support].ContainsKey(skillData.skillGroup))
            {
                skillDataTable[(int)SkillType.Support][skillData.skillGroup].AddToGroup(skillData);
            }
            else
            {
                var newGroup = new SkillDataLevelGroup();
                newGroup.AddToGroup(skillData);
                skillDataTable[(int)SkillType.Support].Add(skillData.skillGroup, newGroup);
            }
        }

        //데이터가 항상 순서대로 있을거라 확신할 수 없어 한번 정렬
        foreach (var dic in skillDataTable)
        {
            foreach(var item in dic)
            {
                item.Value.LevelSort();
            }
        }

        for (int i = 0; i < Enum.GetValues(typeof(SkillType)).Length; i++)
        {
            skillListGroupKeys.Add(skillDataTable[i].Keys.ToList());
        }
    }

    public IReadOnlyList<string> GetSkillGroupKeys(SkillType type)
    {
        return skillListGroupKeys[(int)type];
    }


    public SkillData GetSkillData(SkillType type, string skillGroup, int level)
    {
        return skillDataTable[(int)type][skillGroup].GetSkill(level);
    }

    //public SkillData GetSkillData(int index, int level)
    //{
    //    return GetSkillData(SkillGroupKeys[index], level);
    //}


    public ISkill CreateSkill(SkillData skillData)
    {
        if (skillData.skillType == SkillType.Attack)
        {
            return CreateAttackSkill(skillData as AttackSkillData);

        }
        else if (skillData.skillType == SkillType.Support)
        {
            return CreateSupportSkill(skillData as SupportSkillData);
        }
        else
        {
            return null;
        }
    }

    public ProjectileSkill CreateAttackSkill(AttackSkillData skillData)
    {
        return new ProjectileSkill(skillData);
    }

    public SupportSkill CreateSupportSkill(SupportSkillData skillData)
    {
        switch (skillData.skillTarget)
        {
            case SupportSkillTarget.AttackPower:
                {
                    return new AttackPowerSupportSkill(skillData);
                }
            case SupportSkillTarget.CoolDownTime:
                {
                    return new CoolDownSupportSkill(skillData);
                }
            case SupportSkillTarget.Experience:
                {
                    return new ExperienceSupportSkill(skillData);
                }

            case SupportSkillTarget.ElementalFirePower:
            case SupportSkillTarget.ElementalIcePower:
            case SupportSkillTarget.ElementalThunderPower:
                {
                    return new ElementalAttackPowerSupportSkill(skillData);
                }

        }
        return new AttackPowerSupportSkill(skillData);
    }

}
