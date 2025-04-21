using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatusEffect : StatusEffect
{
    private DamageableStatus target;

    //임시
    public float effectInterval = 2f;
    public int damageCount = 5;
    public int damage;

    private Coroutine coPreformBurnEffect = null;

    private int previousSkillId = -1;
    public override bool CanPerform
    {
        get => true;
    }
    public override bool IsPerforming
    {
        get => isPerforming;
    }
    private bool isPerforming;

    private void Start()
    {
        SetDamagerableTarget(GetComponent<DamageableStatus>());
    }

    public override void SetDamagerableTarget(DamageableStatus damageable)
    {
        target = damageable;
    }

    private bool CanPerformID(int skillID)
    {
        return previousSkillId != skillID;
    }

    public override void Perform(int skillID)
    {
        if (!CanPerformID(skillID))
        {
            return;
        }

        previousSkillId = skillID;

        if (coPreformBurnEffect != null)
        {
            StopCoroutine(coPreformBurnEffect);
        }

        isPerforming = true;
        coPreformBurnEffect = StartCoroutine(CoPerformBurnEffect());
    }


    private IEnumerator CoPerformBurnEffect()
    {
        for (int i = 0; i < damageCount; i++)
        {
            Debug.Log($"화상 효과 damage {i}번째 : {damage}");

            target.OnDamage(damage);
            yield return new WaitForSeconds(effectInterval);
        }
        isPerforming = false;
    }
}
