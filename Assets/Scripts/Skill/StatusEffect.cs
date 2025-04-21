using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public abstract void SetDamagerableTarget(DamageableStatus damageable);

    public abstract bool CanPerform
    {
        get;
    }

    public abstract bool IsPerforming
    {
        get;
    }

    public abstract void Perform(int skillID);
}
