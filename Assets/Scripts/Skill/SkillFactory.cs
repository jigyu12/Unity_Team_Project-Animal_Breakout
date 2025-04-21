using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory
{
    public ISkill CreateSkill(SkillData skillData)
    {
        if (skillData.skillType == SkillType.Attack)
        {
            return CreateProjectileSkill(skillData as ProjectileSkillData);

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

    public ProjectileSkill CreateProjectileSkill(ProjectileSkillData skillData)
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
            case SupportSkillTarget.CoolDown:
                {
                    return new CoolDownSupportSkill(skillData);
                }
            case SupportSkillTarget.Experience:
                {
                    return new ExperienceSupportSkill(skillData);
                }

        }
        return new AttackPowerSupportSkill(skillData);
    }

}
