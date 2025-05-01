using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamagableStatus : DamageableStatus
{

    public override float currentHp { get; protected set; }
    public override float maxHp { get; protected set; }
    public override bool isDead { get; protected set; }

    public override void InitializeStatus(float maxHp)
    {
        this.maxHp = maxHp;
        currentHp = maxHp;
        isDead = false;
    }

    public override void OnDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.Assert(false, "Invalid Damage value");

            return;
        }

        if (isDead)
        {
            return;
        }

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
    }

    public override void OnDamage(float damage, SkillElemental attribute)
    {
        onElementalDamaged?.Invoke(damage, attribute);
        OnDamage(damage);
    }

    protected override void OnDead()
    {
       //몰루
    }

}
