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


    public override void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker = null, DamageableStatus target = null)
    {
        base.Perform(attackerTrs, targetTrs, attacker, target);

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
