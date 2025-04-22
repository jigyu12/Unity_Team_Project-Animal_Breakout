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
        base.Perform(attackerTrs, targetTrs, attacker, target);

        targetStatus = attacker.gameObject.GetComponent<ExperienceStatus>();
        targetStatus.AddAdditionalExperienceRateValue(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        if (Level >= ISkill.maxLevel)
        {
            return;
        }

        targetStatus.AddAdditionalExperienceRateValue(-SupportSkillData.rate);
        base.UpgradeLevel();
        targetStatus.AddAdditionalExperienceRateValue(SupportSkillData.rate);
    }
}
