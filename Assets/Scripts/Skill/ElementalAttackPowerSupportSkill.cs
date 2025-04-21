using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttackPowerSupportSkill : SupportSkill
{
    private AttackPowerStatus targetStatus;
    private SkillElemental elemental;
    public ElementalAttackPowerSupportSkill(SupportSkillData supportSkillData, SkillElemental elemental) : base(supportSkillData)
    {
        this.elemental = elemental;
    }


    public override void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker = null, DamageableStatus target = null)
    {
        targetStatus = attacker;
        targetStatus.SetElementalAdditionalAttackPowerRateValue(elemental, SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        //기존 additional 값을 지우고 수정해야한다.
    }
}
