using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricStatusEffect : StatusEffect
{

    private DamageableStatus target;

    //임시
    public float effectInterval = 2f;
    public int stackMaxCount = 6;
    private int currentStackCount;
    public int damage;

    public override bool CanPerform
    {
        get=> currentStackCount >= stackMaxCount;
    }

    public override bool IsPerforming
    {
        get=> isPerforming;
    }
    private bool isPerforming;

    private void Start()
    {
        SetDamagerableTarget(GetComponent<DamageableStatus>());
        isPerforming = false;
        currentStackCount = stackMaxCount;
    }

    public override void SetDamagerableTarget(DamageableStatus damageable)
    {
        target = damageable;
        target.onDamaged += AddElectricStack;
    }

    //스택은 번개 속성 스킬을 맞췄을 때 1스택씩 쌓이고 6스택을 쌓으면 번개 속성 타겟팅 추가 스킬이 발동된다.
    public override void Perform()
    {
        if (CanPerform)
        {
            isPerforming = true;
            currentStackCount = 0;
        }
    }

    private void AddElectricStack(float damage, SkillElemental attribute)
    {
        if (attribute != SkillElemental.Electricity || !IsPerforming)
        {
            return;
        }

        currentStackCount++;
        if (currentStackCount >= stackMaxCount)
        {
            PerformElectricEffect();
        }
    }

    private void PerformElectricEffect()
    {
        isPerforming = false;
        Debug.Log($"전기 효과 damage : {damage}");
        target.OnDamage(damage, SkillElemental.Electricity);
    }

}
