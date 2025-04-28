using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSupportSkill : SupportSkill
{
    public CoolDownSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(AttackPowerStatus attacker, DamageableStatus target, Transform start, Transform destination = null)
    {
        base.Perform(attacker, target, start);

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
