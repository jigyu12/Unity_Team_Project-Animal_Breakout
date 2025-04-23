using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionSystem
{
    private SkillManager skillManager;
    private List<ISkill> skills;

    public Action<List<ISkill>> onSkillListUpdated;


    public SkillSelectionSystem(SkillManager manager, List<ISkill> skills)
    {
        skillManager = manager;
        this.skills = skills;
    }

    public bool IsSkillExist(ISkill skill)
    {
        return IsSkillExist(skill.Id);
    }

    public bool IsSkillExist(int skillId)
    {
        return skills.Exists((target) => target.Id == skillId);
    }

    public void AddSkill(int priority, ISkill skill)
    {
        var item = new SkillPriorityItem(priority, skill);
        skills.Add(skill);
        onSkillListUpdated?.Invoke(skills);


        skill.InitializeSkilManager(skillManager);

        if (skill.SkillData.skillType == SkillType.Attack)
        {
            AttackSkill attackSkill = skill as AttackSkill;
            attackSkill.AddOnReadyAction(() => skillManager.AddSkillToReadyQueue(item));
            attackSkill.OnReady();
        }
        else if(skill.SkillData.skillType == SkillType.Support)
        {
            skillManager.PerformSkill(skill);
        }
    }

    public void UpgradeSkill()
    {

    }
}
