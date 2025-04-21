using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerSupportSkill : SupportSkill
{
    private AttackPowerStatus targetStatus;

    public AttackPowerSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker = null, DamageableStatus target = null)
    {
        targetStatus = attackerTrs.gameObject.GetComponent<AttackPowerStatus>();
        targetStatus.SetAdditionalAttackPowerRateValue(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        //기존 additional 값을 지우고 수정해야한다.
    }
}
