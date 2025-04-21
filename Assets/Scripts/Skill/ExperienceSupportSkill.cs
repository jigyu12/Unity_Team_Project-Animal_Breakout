using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSupportSkill : SupportSkill
{
    private ExperienceStatus targetStatus;

    public ExperienceSupportSkill(SupportSkillData supportSkillData):base(supportSkillData)
    {
    }

    public override void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker = null, DamageableStatus target = null)
    {
        targetStatus = attacker.gameObject.GetComponent<ExperienceStatus>();
        targetStatus.SetAdditionalExperienceRateValue(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        //기존 additional 값을 지우고 수정해야한다.
    }
}
