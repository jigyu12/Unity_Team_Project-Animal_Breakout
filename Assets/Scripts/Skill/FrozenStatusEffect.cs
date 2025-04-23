using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenStatusEffect : StatusEffect
{
    private readonly int effectId = 1602;
    private DamageableStatus target;

    //임시
    public int damage;

    public int effectCount = 1;
    private int currentCount;

    public override bool CanPerform
    {
        get => !IsPerforming;
    }
    public override bool IsPerforming
    {
        get => isPerforming;
    }
    private bool isPerforming;

    private void Start()
    {
        SetEffectData(effectId);
        SetDamagerableTarget(GetComponent<DamageableStatus>());
        isPerforming = false;
    }

    public override void SetDamagerableTarget(DamageableStatus damageable)
    {
        target = damageable;
        target.onElementalDamaged += PerformFrozenEffect;
    }

    //냉기 상태에서는 얼음 속성을 제외한 다른 속성 스킬로 공격을 했을 때 얼음 속성 1회 추가 데미지를 주는 효과를 갖고 있다.
    public override void Perform(int skillID)
    {
        if (CanPerform)
        {
            isPerforming = true;
            currentCount = 0;
        }
    }

    private void PerformFrozenEffect(float damage, SkillElemental attribute)
    {
        if (attribute == SkillElemental.Ice || !IsPerforming)
        {
            return;
        }

        currentCount++;

        Debug.Log($"얼음 효과 damage : {this.damage}");
        target.OnDamage(this.damage);

        if (currentCount >= effectCount)
        {
            isPerforming = false;
        }
    }
}
