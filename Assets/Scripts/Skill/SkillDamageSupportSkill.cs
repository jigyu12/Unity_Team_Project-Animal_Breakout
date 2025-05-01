using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageSupportSkill : SupportSkill
{
    private AttackPowerStatus targetStatus;

    public SkillDamageSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(AttackPowerStatus attacker, DamageableStatus target, Transform start, Transform destination = null)
    {
        base.Perform(attacker, target, start);
        targetStatus = attacker;
        targetStatus.AddAdditionalSkillAttackPowerRateValue(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        if (Level >= ISkill.maxLevel)
        {
            return;
        }

        targetStatus.AddAdditionalSkillAttackPowerRateValue(-SupportSkillData.rate);
        base.UpgradeLevel();
        targetStatus.AddAdditionalSkillAttackPowerRateValue(SupportSkillData.rate);
    }
}
