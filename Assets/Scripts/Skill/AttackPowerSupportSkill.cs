using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerSupportSkill : SupportSkill
{
    private AttackPowerStatus targetStatus;

    public AttackPowerSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(AttackPowerStatus attacker, DamageableStatus target, Transform start, Transform destination = null)
    {
        base.Perform(attacker, target, start);
        targetStatus = attacker;
        targetStatus.AddAdditionalAttackPowerRateValue(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        if (Level >= ISkill.maxLevel)
        {
            return;
        }

        targetStatus.AddAdditionalAttackPowerRateValue(-SupportSkillData.rate);
        base.UpgradeLevel();
        targetStatus.AddAdditionalAttackPowerRateValue(SupportSkillData.rate);
    }
}
