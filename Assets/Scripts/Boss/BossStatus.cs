using UnityEngine;

public class BossStatus : DamageableStatus
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
        
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
        
        Debug.Log($"Boss HP : {currentHp}/{maxHp}");

        if (Mathf.Approximately(0f, currentHp))
        {
            Debug.Log("Boss is dead.");
            
            OnDead();
        }
    }

    protected override void OnDead()
    {
        Destroy(gameObject);
    }
}