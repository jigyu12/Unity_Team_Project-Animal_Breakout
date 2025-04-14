using UnityEngine;

public abstract class DamageableStatus : MonoBehaviour, IDamageable
{
    public abstract float currentHp { get; protected set; }
    public abstract float maxHp { get; protected set; }
    
    public abstract bool isDead { get; protected set; }

    public abstract void InitializeStatus(float maxHp);
    public abstract void OnDamage(float damage);
    protected abstract void OnDead();
}