using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttackPowerSupportSkill : SupportSkill
{
    private AttackPowerStatus targetStatus;

    public SkillElemental TargetElental
    {
        get;
        private set;
    }

    public ElementalAttackPowerSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
        TargetElental = (SkillElemental)(SupportSkillData.skillTarget - SupportSkillTarget.ElementalFirePower);
    }


    public override void Perform(AttackPowerStatus attacker, DamageableStatus target, Transform start, Transform destination = null)
    {
        base.Perform(attacker, target, start);

        targetStatus = attacker;
        targetStatus.AddElementalAdditionalAttackPowerRateValue(TargetElental, SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        if (Level >= ISkill.maxLevel)
        {
            return;
        }

        targetStatus.AddElementalAdditionalAttackPowerRateValue(TargetElental, -SupportSkillData.rate);
        base.UpgradeLevel();
        targetStatus.AddElementalAdditionalAttackPowerRateValue(TargetElental, SupportSkillData.rate);
    }
}
