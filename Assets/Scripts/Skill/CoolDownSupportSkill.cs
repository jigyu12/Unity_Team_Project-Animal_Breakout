using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownSupportSkill : SupportSkill
{
    public CoolDownSupportSkill(SupportSkillData supportSkillData) : base(supportSkillData)
    {
    }

    public override void Perform(Transform attackerTrs, Transform targetTrs, IAttacker attacker = null, DamageableStatus target = null)
    {
        float previousValue = skillManager.GlobalCoolDownTimeRate;
        skillManager.SetGlobalCoolDownRate(previousValue + previousValue * SupportSkillData.rate);
    }

    public override void UpgradeLevel()
    {
        //기존 additional 값을 지우고 수정해야한다.
    }
}
