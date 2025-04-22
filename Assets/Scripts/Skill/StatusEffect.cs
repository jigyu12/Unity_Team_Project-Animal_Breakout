using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public abstract void SetDamagerableTarget(DamageableStatus damageable);

   

    public AdditionalStatusEffectData AdditionalStatusEffectData
    {
        get;
        private set;
    }

    public abstract bool CanPerform
    {
        get;
    }

    public abstract bool IsPerforming
    {
        get;
    }

    public void SetEffectData(int effectId)
    {
        AdditionalStatusEffectData = DataTableManager.additionalStatusEffectDataTable.Get(effectId);
    }

    public abstract void Perform(int skillID);
}
