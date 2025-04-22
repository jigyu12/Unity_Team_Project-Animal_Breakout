using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSupportSkill : SupportSkill
{
    public CoolDownSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(Transform attackerTrs, Transform targetTrs, AttackPowerStatus attacker = null, DamageableStatus target = null)
    {
        base.Perform(attackerTrs, targetTrs, attacker, target);

        skillManager.AddGlobalCoolDownRate(SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        if (Level >= ISkill.maxLevel)
        {
            return;
        }

        skillManager.AddGlobalCoolDownRate(-SupportSkillData.rate);
        base.UpgradeLevel();
        skillManager.AddGlobalCoolDownRate(SupportSkillData.rate);
    }
}
