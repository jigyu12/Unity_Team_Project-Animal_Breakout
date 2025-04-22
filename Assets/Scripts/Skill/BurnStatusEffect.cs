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

    public override void Perform()
    {
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
            Debug.Log($"화상 효과 damage : {damage}");

            target.OnDamage(damage);
            yield return new WaitForSeconds(effectInterval);
        }
        isPerforming = false;
    }
}
