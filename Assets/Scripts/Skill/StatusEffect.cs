using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour, IDamageable
{
    public abstract void OnDamage(float damage);
    public abstract void Perform();
}
