using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatusEffect : StatusEffect
{

    public float effectInterval = 2f;
    public int damageCount = 5;

    private Coroutine coPreformBurnEffect = null;

    public override void OnDamage(float damage)
    {
       
    }

    public override void Perform()
    {
        if(coPreformBurnEffect!=null)
        {

        }
    }

    //private IEnumerator CoPerformBurnEffect()
    //{

    //}
}
